namespace QuanLySinhVien.DTOS.Respone
{
    public class DMRes
    {
        public string? MaDm { get; set; }
        public string? tenDM{ get; set; }
    }
    public class DanhMucRespone
    {
        public string? MaLoaiDm { get; set; }
        public string? ten { get; set; }
        public List<DMRes> danhMucCon { get; set; } = new List<DMRes>();
    }
}