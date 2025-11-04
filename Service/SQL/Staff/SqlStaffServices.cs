using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.Service.SQL.StaffF
{
    public class SqlStaffServices : SqLServiceBase,  ISqlStaffServices
    {
        private readonly IPassWordService passWordService;
        public SqlStaffServices(MyDbContext context, ILoggerFactory logger, IPassWordService passWordService)
        : base(context, logger)
        {
            this.passWordService = passWordService;
        }

        public async Task<string> AssignUserToStaff(string StaffId, string UserName)
        {
            try
            {
                await context.Database.ExecuteSqlInterpolatedAsync($"""
                    Exec management.uspAssignUserToStaff 
                    @UserName = {UserName},
                    @TargetStaffId = {StaffId}
                """);

                return "SuccesFully Assign User To Staff";
            }
            catch (SqlException)
            {
                throw;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<Staff?> createStaff(Staff newStaff, string imgPath)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                System.Console.WriteLine("Bắt đầu lưu");
                Staff moi = new Staff();
                moi.Avatar = imgPath;
                moi.StaffName = newStaff.StaffName;
                moi.RoleId = newStaff.RoleId;
                moi.StoreId = newStaff.StaffId;
                moi.PhoneNum = newStaff.PhoneNum;
                moi.Email = newStaff.Email;
                moi.StaffId = GenerateId(10, "ST");
                moi.StaffAddr = newStaff.StaffAddr;
                moi.Salary = newStaff.Salary;
                moi.IdNumber = newStaff.IdNumber;
                moi.DoB = newStaff.DoB;
                moi.Gender = newStaff.Gender;
                moi.Status = "Hoạt động";
                moi.StoreId = newStaff.StoreId;
                await context.Staff.AddAsync(moi);
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                System.Console.WriteLine("Lưu Thành Công");
                return moi;
            }
            catch (System.Exception)
            {
                Transaction.Rollback();
                return null;
            }
        }
        public async Task<int> SoftDeleteUser(string Id)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await context.Staff.FindAsync(Id);
                if (user != null)
                {
                    user.Status = "Nghỉ việc";
                    await context.SaveChangesAsync();
                    await Transaction.CommitAsync();
                    return 200;
                }
                return 404;
            }
            catch (Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<int> UpdateStaffInfo(UpdateStaffRequest StaffNewInfo)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var Staff = await context.Staff.FindAsync(StaffNewInfo.StaffId);
                if (Staff == null)
                {
                    return 404;
                }

                Staff.StaffName = StaffNewInfo.StaffName;
                Staff.IdNumber = StaffNewInfo.StaffIdNumber;
                Staff.StaffAddr = StaffNewInfo.Address;
                Staff.DoB = StaffNewInfo.dob;
                Staff.Email = StaffNewInfo.Email;
                Staff.PhoneNum = StaffNewInfo.PhoneNum;
                Staff.Gender = StaffNewInfo.Gender;
                Staff.RoleId = StaffNewInfo.roleid;
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return 200;
            }
            catch(System.Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }
            
        }
    }
}