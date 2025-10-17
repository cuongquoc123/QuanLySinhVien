namespace QuanLySinhVien.Service.GGService
{
    public interface ISheetService
    {
        Task<List<List<string>>> StoreReview(string storeId);
    }
}