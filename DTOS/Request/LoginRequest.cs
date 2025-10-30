namespace QuanLySinhVien.DTOS.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string StoreId { get; set; } = null!;
    }
}