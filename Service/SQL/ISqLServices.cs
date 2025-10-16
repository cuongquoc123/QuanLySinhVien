using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;


namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        Task<Sysuser> UpdateUser(Sysuser sysuser);
        Task<int> deleteUser(string Id);
        Task<int> SoftDeleteUser(string Id);
        Task<Donhang?> taoDon(string CuaHangId, string MaNV, List<Product> dssp, decimal ThanhTien);
        Task<Sanpham?> CreateProDucts(Sanpham spMoi, string imgPath);
        Task<int> SoftDeleteProduct(string productId);
    }
}