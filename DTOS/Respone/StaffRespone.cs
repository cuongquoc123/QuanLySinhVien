namespace QuanLySinhVien.DTOS.Respone
{
    public class StaffRespone
    {
        public string? staffId { get; set; }
        public string? cccd { get; set; }
        public string? ten { get; set; } 
        public string? diaChi { get; set; }
        public DateOnly ngaySinh    { get; set; }
        public decimal luong { get; set; }
        public decimal thuong { get; set; }
        public string? avatar { get; set; }
        public string? statuSf { get; set; } 
        public string? cuaHangId { get; set; }
    }
}