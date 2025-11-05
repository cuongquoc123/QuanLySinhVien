namespace QuanLySinhVien.DTOS.SqlDTO
{
    public class StoreAccountResult
    {
        public string Username { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public string? StaffName { get; set; }
        public string? StaffId { get; set; }
        public string? EligibleStaff { get; set; }
    }
}