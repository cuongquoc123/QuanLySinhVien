using QuanLySinhVien.DTOS.Respone;

namespace QuanLySinhVien.DTOS.Respone
{
    public class CTDHRespone
    {
        public string? masp { get; set; }
        public string? tenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public decimal ThanhTiem { get; set; }
    }
    public class Item2
    {
        public string? maDon { get; set; }
        public string? TrangTHai { get; set; }
        public DateTime? NgayNhan { get; set; }
        public DateTime? NgayHoangThanh { get; set; }
        public string? User { get; set; }
        public List<CTDHRespone> CTDH { get; set; } = new List<CTDHRespone>();  
        public string? PathChiTiet { get; set; }

        
    }
    public class PageRespone2
    {
        public int PageIndex { get; set; } // Số trang hiện tại
        public int PageSize { get; set; }  // Số lượng bản ghi trên mỗi trang
        public int TotalPages { get; set; } // Tổng số trang có thể có
        public int TotalCount { get; set; } // Tổng số bản ghi trong DB
        public List<Item2> Items { get; set; } = new List<Item2>(); // Danh sách các bản ghi của trang hiện tại

        // Thuận tiện để biết liệu có trang trước hoặc trang tiếp theo không
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}