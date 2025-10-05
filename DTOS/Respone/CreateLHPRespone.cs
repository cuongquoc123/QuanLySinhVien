namespace QuanLySinhVien.DTOS.Respone
{
    public class CreateLHPRepone
    {
        public List<string> Danh_Sach_Lop_Tao_Thanh_Cong { get; set; }
        public string? So_luong_lop { get; set; }

        public CreateLHPRepone()
        {
            Danh_Sach_Lop_Tao_Thanh_Cong =new List<string>();
        }
    }
}