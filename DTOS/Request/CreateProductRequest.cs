namespace QuanLySinhVien.DTOS.Request
{
    public class CreateProductRequest
    {
        public string? productname { get; set; }
        public string? DMID { get; set; }
        public string? mota { get; set; }
        public decimal donGia { get; set; }
        public IFormFile file { get; set; } = null!;
    }
}