
using QuanLySinhVien.Models;

namespace QuanLySinhVien.DTOS.Request
{
    public class ProductItem
    {
        public string? Masp { get; set; }
        public int SoLuong { get; set; }
    }
    public class HoaDonRequest
    {
        public List<ProductItem> dssp { get; set;}
        public string? makhach {  get; set; }
        public HoaDonRequest()
        {
            dssp = new List<ProductItem>();
        }        
    }
}