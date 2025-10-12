
using QuanLySinhVien.Models;

namespace QuanLySinhVien.DTOS.Request
{
    public class Product
    {
        public Sanpham? sp { get; set; }
        public int SoLuong { get; set; }
    }
    public class HoaDonRequest
    {
        public List<Product> dssp { get; set;}
        public string? MaNV { get; set; }
        public string? MaCH { get; set; }
        
        public Decimal ThanhTien {  get; set; }
        public HoaDonRequest()
        {
            dssp = new List<Product>();
        }        
    }
}