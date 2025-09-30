using GeneticSharp;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.Schedule;

namespace QuanLySinhVien.Service.SQL
{
    public class SqLService : ISqLServices
    {
        private readonly MyDbContext context;


        public SqLService()
        {
            this.context = new MyDbContext();
        }

        public SqLService(MyDbContext context)
        {
            this.context = context;
        }

        public LichHoc AddLichHoc(string DayOfWeek, Schedulee schedule)
        {
            LichHoc moi = new LichHoc();
            moi.DayOfWeek = DayOfWeek;
            moi.PhongHoc = schedule.PhongHoc;
            moi.SoTiet = schedule.Sotiet;
            moi.TietBatDau = schedule.Slot;
            moi.MaLopHp = schedule.Classes.MaLopHp;
            context.LichHocs.Add(moi);
            context.SaveChanges();
            return moi;
        }

        private string GenerateMa(int so_luong_chu, string ky_tu_bat_dau)
        {
            string KQ = ky_tu_bat_dau;
            for (int chu = ky_tu_bat_dau.Length - 1; chu <= so_luong_chu - ky_tu_bat_dau.Length; chu++)
            {
                int chuCaiMoi = RandomizationProvider.Current.GetInt(65, 91);
                int chuCaiMoi2 = RandomizationProvider.Current.GetInt(97, 123);
                int chon = RandomizationProvider.Current.GetInt(0, 2);
                if (chon == 1)
                {
                    KQ += (char)chuCaiMoi;
                }
                else
                {
                    KQ += (char)chuCaiMoi2;
                }
            }
            return KQ;
        }


    }
}