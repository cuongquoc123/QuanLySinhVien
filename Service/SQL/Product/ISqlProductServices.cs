using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.ProductS
{
    public interface ISqlProductServiecs
    {
        Task<Product?> CreateProDucts(Product spMoi, string imgPath);
        Task<int> SoftDeleteProduct(string productId);
    }
}