using GeneticSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.Service.SQL
{
    public class SqLService : ISqLServices
    {
        private readonly MyDbContext context;

        private readonly ILogger<SqLService> logger;

        private readonly IPassWordService passWordService;
        private string GenerateId(int so_luong_chu, string KyTuBatDau)
        {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(KyTuBatDau))
            {
                id += KyTuBatDau;
            }
            for (int i = 0; i < so_luong_chu - KyTuBatDau.Length; i++)
            {
                Random ran = new Random();
                int chuKeTiep1 = ran.Next(65, 91);
                int chuKeTiep2 = ran.Next(97 , 123);
                int luaChon = ran.Next(0, 2);
                if (luaChon == 1)
                {
                    id += (char)chuKeTiep1;
                }
                else
                {
                    id += (char)chuKeTiep2;
                }
            }
            return id;
            
        } 
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

        public async Task<int> SoftDeleteUser(string Id)
        {
            await using var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await context.Sysusers.FindAsync(Id);
                if (user != null)
                {
                    
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

        public async Task<Donhang?> taoDon(string CuaHangId, string MaNV, List<Product> dssp, decimal ThanhTien)
        {
            await using var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Donhang moi = new Donhang();
                moi.MaDon = this.GenerateId(so_luong_chu: 10, KyTuBatDau: "DH");
                moi.CuaHangId = CuaHangId;
                moi.UserId = MaNV;
                moi.NgayNhan = DateOnly.FromDateTime(DateTime.Now);
                moi.TrangThai = "Tiep Nhan";
                moi.ThanhTien = ThanhTien;
                context.Donhangs.Add(moi);
                foreach (var sp in dssp)
                {
                    if (string.IsNullOrEmpty(sp.Masp))
                    {
                        throw new Exception("A Product in list Product null");
                    }
                    ChiTietDonHang mois = new ChiTietDonHang();
                    mois.MaDon = moi.MaDon;
                    mois.MaSp = sp.Masp;
                    mois.SoLuong = sp.SoLuong;
                    
                    context.ChiTietDonHangs.Add(mois);
                }
                
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return moi;
            }
            catch (System.Exception)
            {
                await Transaction.RollbackAsync();
                return null;
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