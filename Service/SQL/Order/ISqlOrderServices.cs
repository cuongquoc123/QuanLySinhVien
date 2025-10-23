using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Order
{
    public interface IOrderService
    {
        Task<Donhang?> taoDon( string MaNV, List<Product> dssp, string makhach);
        Task<Donhang?> updateDonStatus(string madon, string status);
    }
}