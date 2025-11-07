namespace QuanLySinhVien.DTOS.Request
{
    public class UpdateStockRequest
    {
        public string GoodId { get; set; } = null!;
        public int NewStock { get; set; }
    }
}