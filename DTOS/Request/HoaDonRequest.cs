
using QuanLySinhVien.Models;

namespace QuanLySinhVien.DTOS.Request
{
    public class HoaDonRequest
    {
        public List<Sanpham> dssp { get; set; }

        public string? MaNV { get; set; }
        public string? MaCH { get; set; }
        
        public Decimal ThanhTien {  get; set; }
        public HoaDonRequest()
        {
            dssp = new List<Sanpham>();
        }        
    }
}