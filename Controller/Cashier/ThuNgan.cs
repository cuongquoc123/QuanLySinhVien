using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HTMLRaw;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Cashier
{
    [ApiController]
    // [Route("Cashier")]
    public class ThuNgan : ControllerBase
    {
        private readonly IHtmService htmService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly MyDbContext context;
        private readonly ISqLServices sqLServices;
        public ThuNgan(MyDbContext context, ISqLServices sqLServices, IWebHostEnvironment webHostEnvironment, IHtmService htmService)
        {
            this.context = context;
            this.sqLServices = sqLServices;
            this.webHostEnvironment = webHostEnvironment;
            this.htmService = htmService;
        }

        [HttpPost("TaoDon")]
        public async Task<IActionResult> taoDonHang([FromBody] HoaDonRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is empty."); // Trả về lỗi 400 Bad Request
            }
            
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Token is not valid");
                }
                var user = await context.Sysusers.FindAsync(userId);

                if (user == null)
                {
                    throw new UnauthorizedAccessException("User Not Exists in server");
                }
                // 3. Toàn bộ logic xử lý chính đặt trong try-catch
                var donhang = await sqLServices.taoDon(
                    ThanhTien: request.ThanhTien,
                    CuaHangId: user.CuaHangId, // Tên thuộc tính nên nhất quán
                    MaNV: userId,
                    dssp: request.dssp
                );
                
                if (donhang == null)
                {
                    
                    // Trả về lỗi 500 Internal Server Error, vì đây là lỗi logic phía server
                    throw new Exception("Failed to create order in database.");
                    
                }

                var res = htmService.HoaDonHTMl(donhang.MaDon);
                if (string.IsNullOrEmpty(res))
                {
                    return StatusCode(500, "Failed to generate HTML bill.");
                }

                return Content(res, "text/html; charset=utf-8");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    
}