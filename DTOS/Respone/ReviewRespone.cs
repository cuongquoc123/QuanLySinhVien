namespace QuanLySinhVien.DTOS.Respone
{
    public class Review
    {
        public int Rating { get; set; }
        public string? comment { get; set; }
        public string? Staffs { get; set; }
        public string? Time { get; set; }
        public string? Custommer { get; set; }
    }
    public class ReviewRespone
    {
        public string? StoreId { get; set; }

        public List<Review> reviews{ get; set; } = new List<Review>();
    }
}