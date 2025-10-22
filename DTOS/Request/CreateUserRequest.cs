namespace QuanLySinhVien.DTOS.Request
{
    public class CreateUserRequest
    {
        public string? Ten { get; set; }  
        public string? DiaChi { get; set; }
        public string? Cccd { get; set; }
        public string? Vtri { get; set; }
        public string? NgaySinh { get; set; }
        public decimal Luong { get; set; }
    }
}