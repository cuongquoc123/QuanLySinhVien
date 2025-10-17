namespace QuanLySinhVien.DTOS.Request
{
    public class CreateStaffRequest
    {
        public string? Cccd { get; set; }
        public string? Ten { get; set; }
        public string? DiaChi { get; set; }
        public string? Vtri { get; set; }
        public string? CuaHangId { get; set; }
        public decimal Luong { get; set; }    
        public string? NgaySinh { get; set; }
    }
}