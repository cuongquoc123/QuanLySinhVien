using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;


namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        public Task<Sysuser> UpdateUser(Sysuser sysuser);

        public Task<int> deleteUser(string Id);

        public Task<int> SoftDeleteUser(string Id);

        Task<Donhang?> taoDon(string CuaHangId, string MaNV, List<Sanpham> dssp, decimal ThanhTien);
    }
}