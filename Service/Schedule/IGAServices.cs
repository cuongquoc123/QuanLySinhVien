using GeneticSharp;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.Schedule
{
    public interface IGAServices
    {
        public Gene[] Ga(int slot, List<LopHocPhan> classrooms, List<string> PhongHoc);


    }
}