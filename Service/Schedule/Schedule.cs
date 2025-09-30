using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.Schedule
{


    public class Schedulee
    {
        public Schedulee(int slot, string PhongHoc, int Sotiet)
        {
            Slot = slot;
            this.PhongHoc = PhongHoc;
            this.Sotiet = Sotiet;
        }

        public Schedulee()
        { 
            Sotiet = Classes.MaMonNavigation.SoTiet;
        }
        public int Slot { get; set; } // Tượng trưng cho ca học 

        public int Sotiet { get; set; }
        public LopHocPhan Classes { get; set; } = new LopHocPhan();

        public string? PhongHoc { get; set; }
    }
}