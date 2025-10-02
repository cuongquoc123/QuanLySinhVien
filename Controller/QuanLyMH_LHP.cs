using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller
{
    [ApiController]
    public class QuanLyMH_LHP : ControllerBase
    {
        private readonly ISqLServices sqLServices;
        private readonly ILogger<QuanLyMH_LHP> logger;
        public QuanLyMH_LHP(ISqLServices sqLServices, ILogger<QuanLyMH_LHP> logger)
        {
            this.sqLServices = sqLServices;
            this.logger = logger;
        }

        [HttpPost("TaoLHP")]
        public IActionResult CreateLHP(List<CreateLHPRequest> dsLHP)
        {
            List<String> dstaoTC = new List<String>();
            foreach (var LHP in dsLHP)
            {
                if (string.IsNullOrEmpty(LHP.TenLopHp) ||
                    string.IsNullOrEmpty(LHP.MaMon) ||
                    string.IsNullOrEmpty(LHP.MaGv) ||
                    string.IsNullOrEmpty(LHP.HocKy) ||
                    string.IsNullOrEmpty(LHP.NamHoc) )
                {
                    continue;
                }
                if (sqLServices.CreateLHP(LHP.TenLopHp,LHP.MaMon,LHP.MaGv,LHP.HocKy,LHP.NamHoc) == 1)
                {
                    dstaoTC.Add(LHP.TenLopHp);
                }
            }
            return Ok(new CreateLHPRepone
            {
                Danh_Sach_Lop_Tao_Thanh_Cong = dstaoTC,
                So_luong_lop = $"{dstaoTC.Count}/{dsLHP.Count}"
            });
        }

    }
}