using Microsoft.AspNetCore.Mvc;
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
            if (string.IsNullOrEmpty(request.MaCH) || string.IsNullOrEmpty(request.MaNV))
            {
                throw new ArgumentException("Need Param User and CuaHang");
            }
            try
            {
                System.Console.WriteLine("Start Create bill");
                // 3. Toàn bộ logic xử lý chính đặt trong try-catch
                var donhang = await sqLServices.taoDon(

                    ThanhTien: request.ThanhTien,
                    CuaHangId: request.MaCH, // Tên thuộc tính nên nhất quán
                    MaNV: request.MaNV,
                    dssp: request.dssp
                );
                System.Console.WriteLine("Done");
                if (donhang == null)
                {
                    System.Console.WriteLine("donhang = null");
                    // Trả về lỗi 500 Internal Server Error, vì đây là lỗi logic phía server
                    throw new Exception("Failed to create order in database.");
                    
                }
                System.Console.WriteLine("Madon: " + donhang.MaDon);
                var res = htmService.HoaDonHTMl(donhang.MaDon);
                System.Console.WriteLine("var res = htmService.HoaDonHTMl(donhang);");
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