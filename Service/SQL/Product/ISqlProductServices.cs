using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.ProductS
{
    public interface ISqlProductServiecs
    {
        Task<Sanpham?> CreateProDucts(Sanpham spMoi, string imgPath);
        Task<int> SoftDeleteProduct(string productId);
    }
}