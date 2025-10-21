using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;


namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        Task<Sysuser> CreateUser(Staff newUser);
        Task<Staff> UpdateUser(Staff sysuser, string Password);
        Task<int> SoftDeleteUser(string Id);
        Task<Donhang?> taoDon( string MaNV, List<Product> dssp, string makhach);
        Task<Donhang?> updateDonStatus(string madon, string status);
        Task<Sanpham?> CreateProDucts(Sanpham spMoi, string imgPath);
        Task<int> SoftDeleteProduct(string productId);
        Task<Cuahang?> CreateStore(Cuahang NewStore);
        Task<int> SoftDeleteStore(string StoreId);
        Task<Staff?> createStaff(Staff newStaff, string imgPath);
    }

}