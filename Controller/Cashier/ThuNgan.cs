using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.Service.SQL.Order;

namespace QuanLySinhVien.Controller.Cashier
{
    [ApiController]
    [Route("api/order")]
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public class ThuNgan : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly IOrderService sqLServices;
        public ThuNgan( IOrderService sqLServices, MyDbContext context )
        {
            this.sqLServices = sqLServices;
            this.context = context;
        }

        [HttpPost("")]
        public async Task<IActionResult> taoDonHang([FromBody] HoaDonRequest request)
        {
            if (request == null )
            {
                return BadRequest("Request body is empty."); // Trả về lỗi 400 Bad Request
            }
            if (string.IsNullOrEmpty(request.makhach) )
            {
                request.makhach = "CTM0000001";
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Token is not valid");
                }

                var donhang = await sqLServices.taoDon(makhach: request.makhach, MaNV: int.Parse(userId), dssp: request.dssp);
                
                if (donhang == null)
                {

                    // Trả về lỗi 500 Internal Server Error, vì đây là lỗi logic phía server
                    throw new Exception("Failed to create order in database.");

                }
                List<HoaDonRespone> ChiTietDonHang = new List<HoaDonRespone>();
                foreach( var item in donhang.OrderDetails)
                {
                    ChiTietDonHang.Add(new HoaDonRespone
                    {
                        tenSP = item.Product.ProductName ,
                        SoLuong = item.Quantity,
                        DonGia = item.Product.Price
                    });
                }  
                return Ok(new
                {
                    donhang = donhang.OrderId,
                    NgayNhan = donhang.RecivingDate,
                    ChiTiet = ChiTietDonHang
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("Unproccess")]
        public async Task<IActionResult> GetOrderUnproccess()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Token not valid");
                }

                var user = await context.Sysusers.FindAsync(int.Parse(userId));

                if (user == null)
                {
                    throw new UnauthorizedAccessException("This is not my Token");
                }

                var respone = new PageRespone2();
                var Items = await context.Orders.Include(ord => ord.SysUser).Include(ord => ord.OrderDetails)
                                .ThenInclude(detail => detail.Product)
                                .Where(d => d.SysUser != null && d.Status == "Tiếp nhận"
                                && d.SysUser.StoreId == user.StoreId).ToListAsync();
                if (Items == null || Items.Count == 0 || !Items.Any())
                {
                    return NoContent();
                }

                foreach (var item in Items)
                {
                    Item2 itemNew = new Item2()
                    {
                        maDon = item.OrderId,
                        TrangTHai = item.Status,
                        NgayNhan = item.RecivingDate,
                        NgayHoangThanh = item.CompleteDate,
                        User = item.SysUser?.UserName,
                        PathChiTiet = $"/manager/DH/chitiet/{item.OrderId}"
                    };
                    
                    foreach (var item2 in item.OrderDetails)
                    {
                        CTDHRespone CTDHS = new CTDHRespone()
                        {
                            masp = item2.ProductId,
                            tenSP = item2.Product.ProductName,
                            SoLuong = item2.Quantity,
                            Gia = item2.Product.Price,
                            ThanhTiem = item2.Quantity * item2.Product.Price
                        };
                        itemNew.CTDH.Add(CTDHS);
                    }
                    
                    
                    respone.Items.Add(itemNew);
                }
                return Ok(respone);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        [HttpPut("Unproccess")]
        public async Task<IActionResult> UpdateDonStatus([FromQuery] string id, [FromQuery] string status)
        {
            System.Console.WriteLine(id);
            System.Console.WriteLine(status);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(status))
            {
                throw new ArgumentException("Missing Parram Madon or Status");
            }
            try
            {
                if (status == "Hủy")
                {
                    status = "Đã hủy";
                }
                var respone = await sqLServices.updateDonStatus(id, status);
                if (respone == null)
                {
                    throw new Exception("Can't update status in database");
                }
                return Ok(new
                {
                    message = "Update status succes fully"
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
    
}