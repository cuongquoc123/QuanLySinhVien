using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.PhieuNhapKho
{
    public interface ISqlPhieuNhapKho
    {
        Task<Grn?> TaoPhieuNhat(List<ProductItem> dsNL, int Makho);
        Task<Gon?> CreateOrderGoods(List<ProductItem> dsNL, int InventoryId);
    }
} 