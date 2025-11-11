using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.PhieuNhapKho
{
    public interface ISqlPhieuNhapKho
    {
        Task<Inventoryrecord?> CreateInventoryRecords(List<ProductItem> dsNL, int Inventory,int RecordsType);
    }
} 