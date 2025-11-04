namespace QuanLySinhVien.DTOS.Request
{
    public class UpdateStaffRequest
    {
        public string StaffId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string StaffIdNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateOnly dob { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNum { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string roleid { get; set; } = null!;
    }
}