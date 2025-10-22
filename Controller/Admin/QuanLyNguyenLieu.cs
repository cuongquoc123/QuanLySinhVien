using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL.NguyenLieu;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("admin/NL")]
    public class QuanLyNguyenLieu : ControllerBase
    {
        private readonly ISqlNguyenLieuServices sqlNguyenLieuServices;
        private readonly MyDbContext context;

        public QuanLyNguyenLieu(ISqlNguyenLieuServices sqlNguyenLieuServices, MyDbContext context)
        {
            this.context = context;
            this.sqlNguyenLieuServices = sqlNguyenLieuServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNL([FromQuery] string TenNL, [FromQuery] string DVT)
        {
            if (string.IsNullOrWhiteSpace(TenNL) || string.IsNullOrWhiteSpace(DVT))
            {
                throw new ArgumentException("Missing Query 'TenNL' or 'DVT' ");
            }
            try
            {
                var respone = await sqlNguyenLieuServices.taoNguyenLieu(tenNL: TenNL, DVT: DVT);

                if (respone == null)
                {
                    throw new Exception("Can't create NL");
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