using GeneticSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.Service.SQL
{
    public class SqLService : ISqLServices
    {
        private readonly MyDbContext context;

        private readonly ILogger<SqLService> logger;

        private readonly IPassWordService passWordService;
        public SqLService(MyDbContext context, ILogger<SqLService> logger, IPassWordService passWordService)
        {
            this.context = context;
            this.logger = logger;
            this.passWordService = passWordService;
        }

        public async Task<int> deleteUser(string Id)
        {
            await using var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await context.Sysusers.FindAsync(Id);
                if (user == null)
                {
                    throw new KeyNotFoundException("Can't find User");
                }
                context.Sysusers.Remove(user);
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return 200;
            }
            catch (System.Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }
        }

        

        public async Task<Sysuser> UpdateUser(Sysuser sysuser)
        {
            await using var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await context.Sysusers.FindAsync(sysuser.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException("Can't Find User");    
                }
                user.Avatar = sysuser.Avatar;
                user.CuaHangId = sysuser.CuaHangId;
                user.DiaChi = sysuser.DiaChi;
                user.NgaySinh = sysuser.NgaySinh;
                user.Passwords = passWordService.HashPassWord(sysuser.Passwords);
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return user;
            }
            catch (System.Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }
        }
    }
}