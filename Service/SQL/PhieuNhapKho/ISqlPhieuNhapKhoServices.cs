using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.PhieuNhapKho
{
    public interface ISqlPhieuNhapKho
    {
        Task<Grn?> TaoPhieuNhat(List<ProductItem> dsNL, string Makho);
         
    }
} 