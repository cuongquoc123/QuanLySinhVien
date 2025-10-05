using QuanLySinhVien.Models;
using QuanLySinhVien.Service.Schedule;

namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        public Task<LichHoc> AddLichHoc(string DayOfWeek, Schedulee schedule);
        public Task DiemDanhThanhCong(string mssv, string maLHP);

        public Task<int> CreateLHP(string TenLopHp, string MaMon, string MaGv, string HocKy, string NamHoc);
    }
}