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
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly MyDbContext context;
        private readonly IOrderService sqLServices;
        public ThuNgan(MyDbContext context, IOrderService sqLServices, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.sqLServices = sqLServices;
            this.webHostEnvironment = webHostEnvironment;
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
                request.makhach = "";
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Token is not valid");
                }
                
                var donhang = await sqLServices.taoDon(
                    makhach: request.makhach,
                    MaNV: userId.Trim(),
                    dssp: request.dssp
                );
                if (donhang == null)
                {

                    // Trả về lỗi 500 Internal Server Error, vì đây là lỗi logic phía server
                    throw new Exception("Failed to create order in database.");

                }
                var customer = await context.CustomerDetails.FindAsync(request.makhach);
                if (customer == null)
                {
                    customer = new CustomerDetail();
                    customer.TenKhach = "Khách vãng lai";
                }
                List<ChiTietDonHang> CTdonhangs = await context.ChiTietDonHangs.Where(ct => ct.MaDon == donhang.MaDon).ToListAsync();
                List<HoaDonRespone> ChiTietDonHang = new List<HoaDonRespone>();
                foreach( var item in CTdonhangs)
                {
                    var sp = await context.Sanphams.FindAsync(item.MaSp);
                    if (sp == null)
                    {
                        continue;
                    }
                    ChiTietDonHang.Add(new HoaDonRespone
                    {
                        tenSP = sp.TenSp ,
                        SoLuong = item.SoLuong,
                        DonGia = sp.DonGia
                    });
                }  
                return Ok(new
                {
                    KhachHang = customer.TenKhach,
                    donhang = donhang.MaDon,
                    NgayNhan = donhang.NgayNhan,
                    ChiTiet = ChiTietDonHang
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{Madon}/{Status}")]
        public async Task<IActionResult> UpdateDonStatus([FromRoute] string Madon, [FromRoute] string Status)
        {
            if (string.IsNullOrEmpty(Madon) || string.IsNullOrEmpty(Status))
            {
                throw new ArgumentException("Missing Parram Madon or Status");
            }
            try
            {
                var respone = await sqLServices.updateDonStatus(Madon, Status);
                if (respone == null)
                {
                    throw new Exception("Can't update status in database");
                }
                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
    
}