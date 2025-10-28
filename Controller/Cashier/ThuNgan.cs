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

        private readonly IOrderService sqLServices;
        public ThuNgan( IOrderService sqLServices)
        {
            this.sqLServices = sqLServices;

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