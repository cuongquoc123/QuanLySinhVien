using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.StaffF
{
    public interface ISqlStaffServices
    {
        Task<Staff?> createStaff(Staff newStaff, string imgPath);
        Task<int> SoftDeleteUser(string Id);
        Task<String> AssignUserToStaff(string StaffId, string UserName);
    }
}