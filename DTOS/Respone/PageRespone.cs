namespace QuanLySinhVien.DTOS.Respone
{
    public class PageRespone<T>
    {
        public int PageIndex { get; set; } // Số trang hiện tại
        public int PageSize { get; set; }  // Số lượng bản ghi trên mỗi trang
        public int TotalPages { get; set; } // Tổng số trang có thể có
        public int TotalCount { get; set; } // Tổng số bản ghi trong DB
        public IEnumerable<T>? Items { get; set; } // Danh sách các bản ghi của trang hiện tại

        // Thuận tiện để biết liệu có trang trước hoặc trang tiếp theo không
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}