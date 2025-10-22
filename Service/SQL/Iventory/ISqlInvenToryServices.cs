using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Iventory
{
    public interface ISQLInventoryService
    {
        Task<Kho?> taoKho(string maCH, string DiaChi);
        Task<Kho?> softDeleteKho(string maKho);
    }
}