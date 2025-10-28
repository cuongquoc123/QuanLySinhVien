using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Iventory
{
    public interface ISQLInventoryService
    {
        Task<Inventory?> taoKho(string maCH, string DiaChi);
        Task<Inventory?> softDeleteKho(string maKho);
    }
}