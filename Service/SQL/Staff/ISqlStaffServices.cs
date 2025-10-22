using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.StaffF
{
    public interface ISqlStaffServices
    {
        Task<Staff?> createStaff(Staff newStaff, string imgPath);
        Task<Sysuser?> createAccount(string staffId, string username, string password,string RoleId);
        Task<int> SoftDeleteUser(string Id);
    }
}