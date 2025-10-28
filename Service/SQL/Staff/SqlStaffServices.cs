using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
                
                Staff moi = new Staff();
                moi.Avatar = imgPath;
                moi.StaffName = newStaff.StaffName;
                moi.RoleId = newStaff.RoleId;
                moi.StoreId = newStaff.StaffId;
                moi.StaffId = GenerateId(10, "ST");
                moi.StaffAddr = newStaff.StaffAddr;
                moi.Salary = newStaff.Salary;
                moi.IdNumber = newStaff.IdNumber;
                moi.DoB = newStaff.DoB;
                await context.Staff.AddAsync(moi);
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
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
            await using var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await context.Staff.FindAsync(Id);
                if (user != null)
                {
                    user.Status = "Nghỉ việc";
                    context.Entry(user).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    await Transaction.CommitAsync();
                    return 200;
                }
                throw new KeyNotFoundException("User Not Exists");
            }
            catch (Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }
        }
        

    }
}