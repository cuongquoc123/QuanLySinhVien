using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.DTOS.SqlDTO;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.StaffF
{
    public interface ISqlStaffServices
    {
        Task<Staff?> createStaff(Staff newStaff, string imgPath);
        Task<int> SoftDeleteUser(string Id);
        Task<String> AssignUserToStaff(string StaffId, string UserName);
        Task<int> UpdateStaffInfo(UpdateStaffRequest StaffNewInfo);
        Task<List<StoreAccount>> GetStoreAccountsAsync(string StoreId, string RoleId);
        Task<PageRespone3<StoreAccount>> GetPageAccountAsync(int PageNum, int PageSize);
    }
}