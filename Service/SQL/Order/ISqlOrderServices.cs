using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Order
{
    public interface IOrderService
    {
        Task<Models.Order?> taoDon( int MaNV, List<ProductItem> dssp, string makhach);
        Task<Models.Order?> updateDonStatus(string madon, string status);
    }
}