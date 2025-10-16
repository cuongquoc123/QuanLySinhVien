namespace QuanLySinhVien.DTOS.Request
{
    public class CreateUserRequest
    {
        public string? UserName { get; set; }  
        public string? Password { get; set; }
        public string? RoleId { get; set; }
        public string? StoreId { get; set; }
    }
}