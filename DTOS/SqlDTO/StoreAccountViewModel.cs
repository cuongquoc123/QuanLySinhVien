namespace QuanLySinhVien.DTOS.SqlDTO
{
    public class StoreAccount
    {
        public string Username { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public string? StaffId { get; set; } 
        public string? StaffName { get; set; }
        public List<EligibleStaff> eligibleStaff { get; set; } = new List<EligibleStaff>();
    }
}