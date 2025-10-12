using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HTMLRaw;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Cashier
{
    [ApiController]
    [Route("Cashier")]
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
            if (string.IsNullOrEmpty(request.MaCH) || string.IsNullOrEmpty(request.MaNV)) {
                throw new ArgumentException("Need Param User and CuaHang");
            }
            var donhang = await sqLServices.taoDon(
                    ThanhTien: request.ThanhTien,
                    CuaHangId: request.MaCH,
                    MaNV: request.MaNV,
                    dssp: request.dssp
            );
            if (donhang == null)
            {
                 throw new Exception("Can't be create Bill");
            }
            var res = htmService.HoaDonHTMl(donhang);
            if (res == null)
            {
                throw new Exception("Can't be create Bill");
            }
            return Content(res, "text/html");
        }
    }
}