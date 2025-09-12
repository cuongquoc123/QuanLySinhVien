using QuanLySinhVien.Models;
using QuanLySinhVien.Service.Schedule;

namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        public LichHoc AddLichHoc(string DayOfWeek, schedule schedule);
    }
}