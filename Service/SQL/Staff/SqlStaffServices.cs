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
        public async Task<Staff?> createStaff(Staff newStaff, string imgPath)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                
                Staff moi = new Staff();
                moi.Avatar = imgPath;
                moi.Ten = newStaff.Ten;
                moi.RoleId = newStaff.RoleId;
                moi.CuaHangId = newStaff.CuaHangId;
                moi.StatuSf = "Hoạt động";
                moi.StaffId = GenerateId(10, "ST");
                moi.DiaChi = newStaff.DiaChi;
                moi.Luong = newStaff.Luong;
                moi.Cccd = newStaff.Cccd;
                moi.NgaySinh = newStaff.NgaySinh;
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
        public async Task<Sysuser?> createAccount(string staffId, string username, string password,string RoleId)
        {
            DbConnection dbConnection = context.Database.GetDbConnection();

            var returnValueParam = new SqlParameter()
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            using (DbCommand command = dbConnection.CreateCommand())
            {
                if (dbConnection.State != ConnectionState.Open)
                    {
                        await dbConnection.OpenAsync();
                    }
                try
                {
                    
                    command.CommandText = "dbo.TaoTaiKhoanStaff";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@StaffId", SqlDbType.Char, 10) { Value = staffId });
                    command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 100) { Value = username });
                    command.Parameters.Add(new SqlParameter("@PassWord", SqlDbType.NVarChar, 100) { Value = passWordService.HashPassWord(password) });
                    command.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.Char, 10) { Value = RoleId });
                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Sysusers.FindAsync(staffId);
                    }

                    throw new Exception("UserName already in db or staff not found");
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                    return null;
                }
                finally
                {
                        await dbConnection.CloseAsync();
                }


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
                    user.StatuSf = "Nghỉ việc";
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