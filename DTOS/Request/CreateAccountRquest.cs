namespace QuanLySinhVien.DTOS.Request
{
    public class createAccountRequest
    {
        public string? AccountId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RoleId { get; set; }
    }
}