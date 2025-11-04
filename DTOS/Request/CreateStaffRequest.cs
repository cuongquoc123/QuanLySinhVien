namespace QuanLySinhVien.DTOS.Request
{
    public class CreateStaffRequest
    {
        public string? Ten { get; set; }
        public string? cccd { get; set;}
        public string? DiaChi { get; set; }
        public string? Gendar { get; set; }
        public string? Email { get; set; }
        public string? PhoneNum { get; set; }
        public string? RoleId { get; set; }
        public decimal Luong { get; set; }   
        public string? cuaHangId { get; set; } 
        public string? NgaySinh { get; set; }
        public IFormFile file { get; set; } = null!;
    }
}