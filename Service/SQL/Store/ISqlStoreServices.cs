using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Store
{
    public interface ISqlStoreServices
    {
        Task<Cuahang?> CreateStore(Cuahang NewStore);
        Task<int> SoftDeleteStore(string StoreId);
    }
}