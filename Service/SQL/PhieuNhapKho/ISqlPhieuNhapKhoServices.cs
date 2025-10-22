using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.PhieuNhapKho
{
    public interface ISqlPhieuNhapKho
    {
        Task<PhieuNhapNl?> TaoPhieuNhat(List<Product> dsNL, string Makho);
         
    }
} 