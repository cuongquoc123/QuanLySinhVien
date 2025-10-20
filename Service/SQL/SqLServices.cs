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
                    user.Status = "Ngưng Hoạt Động";
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
                await context.Donhangs.AddAsync(moi);
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
                    
                    await context.ChiTietDonHangs.AddAsync(mois);
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
                context.Entry(user).State = EntityState.Modified;
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

        public async Task<Sanpham?> CreateProDucts(Sanpham spMoi, string imgPath) 
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                
                Sanpham moi = new Sanpham();
                moi.MaSp = GenerateId(10, "SP");
                moi.TenSp = spMoi.TenSp;
                moi.Status = "Sản phẩm mới";
                moi.Anh = imgPath;
                moi.DonGia = spMoi.DonGia;
                moi.MaDm = spMoi.MaDm;
                moi.Mota = spMoi.Mota;
                await context.Sanphams.AddAsync(moi);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return moi;
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                return null;
            }
            
        }

        public async Task<int> SoftDeleteProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return 500;
            }
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Sanpham? sp = await context.Sanphams.FindAsync(productId);
                if (sp == null)
                {
                    return 404;
                }
                sp.Status = "Ngưng Kinh Doanh";
                context.Entry(sp).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return 200;
            }
            catch (System.Exception)
            {
                await Transaction.RollbackAsync();
                return 500;
            }
        }

        public async Task<Sysuser> CreateUser(Sysuser newUser)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Sysuser USer = new Sysuser();
                USer.UserId = GenerateId(10, "US");
                USer.UserName = newUser.UserName;
                USer.Passwords = passWordService.HashPassWord(newUser.Passwords);
                USer.RoleId = newUser.RoleId;
                USer.CuaHangId = newUser.CuaHangId;
                USer.Status = "New User";
                await context.Sysusers.AddAsync(USer);
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return USer;
            }
            catch (System.Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Cuahang?> CreateStore(Cuahang NewStore)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
           try
           {
                Cuahang moi = new Cuahang();
                moi.CuaHangId = GenerateId(10, "CH");
                moi.TenCh = NewStore.TenCh;
                moi.Statuss = "Đang Hoạt động";
                moi.DiaChi = NewStore.DiaChi;
                moi.Sdt = NewStore.Sdt;
                await context.Cuahangs.AddAsync(moi);
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

        public async Task<int> SoftDeleteStore(string StoreId)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Cuahang? DeleteSore = await context.Cuahangs.FindAsync(StoreId);
                if (DeleteSore == null)
                {
                    return 404;
                }
                DeleteSore.Statuss = "Ngưng hoạt động";
                context.Entry(DeleteSore).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await Transaction.CommitAsync();
                return 200;
            }
            catch (System.Exception)
            {
                await Transaction.RollbackAsync();
                return 500;
            }
        }

        public async Task<Staff?> createStaff(Staff newStaff, string imgPath)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(imgPath))
                {
                    imgPath = "https://bla.edu.vn/wp-content/uploads/2025/09/avatar-fb.jpg";
                }
                Staff moi = new Staff();
                moi.Avatar = imgPath;
                moi.Ten = newStaff.Ten;
                moi.Vtri = newStaff.Vtri;
                moi.CuaHangId = newStaff.CuaHangId;
                moi.StatuSs = "Đang làm";
                moi.StaffId = GenerateId(10, "ST");
                moi.DiaChi = newStaff.DiaChi;
                moi.Luong = newStaff.Luong;
                moi.Cccd = newStaff.Cccd;
                moi.NgaySinh = newStaff.NgaySinh;
                await context.Staff.AddAsync(moi);
                await Transaction.CommitAsync();
                return moi;
            } catch (System.Exception)
            {
                Transaction.Rollback();
                return null;
            }
        }

        public async Task<Donhang?> updateDonStatus(string madon, string status)
        {
            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Donhang? UpdateDon = await context.Donhangs.FindAsync(madon);
                if (UpdateDon == null)
                {
                    throw new KeyNotFoundException("Don Hang not exists");
                }
                UpdateDon.TrangThai = status;
                context.Entry(UpdateDon).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return UpdateDon;
            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
}