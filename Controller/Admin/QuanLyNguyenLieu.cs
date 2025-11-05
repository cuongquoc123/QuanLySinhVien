using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL.NguyenLieu;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("admin/good")]
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
        public async Task<IActionResult> CreateNL([FromQuery] string goods_name, [FromQuery] string unit_name)
        {
            if (string.IsNullOrWhiteSpace(goods_name) || string.IsNullOrWhiteSpace(unit_name))
            {
                throw new ArgumentException("Missing Query 'TenNL' or 'DVT' ");
            }
            try
            {
                var respone = await sqlNguyenLieuServices.taoNguyenLieu(tenNL: goods_name, DVT: unit_name);

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