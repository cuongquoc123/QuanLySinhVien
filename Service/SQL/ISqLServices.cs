using QuanLySinhVien.Models;
using QuanLySinhVien.Service.Schedule;

namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        public LichHoc AddLichHoc(string DayOfWeek, Schedulee schedule);
        public void DiemDanhThanhCong(string mssv, string maLHP);

        public int CreateLHP(string TenLopHp, string MaMon, string MaGv, string HocKy, string NamHoc);
    }
}