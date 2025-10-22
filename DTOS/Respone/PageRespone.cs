namespace QuanLySinhVien.DTOS.Respone
{
    public class Item<T>
    {
        public T? Value{ get; set; }
        public string? PathChiTiet { get; set; }

        
    }
    public class PageRespone<T>
    {
        public int PageIndex { get; set; } // Số trang hiện tại
        public int PageSize { get; set; }  // Số lượng bản ghi trên mỗi trang
        public int TotalPages { get; set; } // Tổng số trang có thể có
        public int TotalCount { get; set; } // Tổng số bản ghi trong DB
        public List<Item<T>> Items { get; set; } = new List<Item<T>>(); // Danh sách các bản ghi của trang hiện tại

        // Thuận tiện để biết liệu có trang trước hoặc trang tiếp theo không
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}