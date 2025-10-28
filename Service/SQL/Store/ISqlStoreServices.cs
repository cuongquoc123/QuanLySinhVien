using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Store
{
    public interface ISqlStoreServices
    {
        Task<Models.Store?> CreateStore(Models.Store NewStore);
        Task<int> SoftDeleteStore(string StoreId);
    }
}