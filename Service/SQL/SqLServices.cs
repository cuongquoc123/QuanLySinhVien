using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using GeneticSharp;
using Microsoft.Data.SqlClient;
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
                int chuKeTiep2 = ran.Next(97, 123);
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
        private DataTable? TaoBangThamSoSanPham(List<Product> dsP)
        {
            if (dsP.Count == 0 || !dsP.Any() || dsP == null)
            {
                return null;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Ma", typeof(String));
            dt.Columns.Add("SoLuong", typeof(int));

            foreach (Product product in dsP)
            {
                dt.Rows.Add(product.Masp, product.SoLuong);
            }
            return dt;
        }
        public async Task<Donhang?> taoDon(string MaNV, List<Product> dssp, string makhach)
        {


            DbConnection dbConnection = context.Database.GetDbConnection();
            using (DbCommand command = dbConnection.CreateCommand())
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    await dbConnection.OpenAsync();
                }

                var returnValueParam = new SqlParameter
                {
                    ParameterName = "@ReturnValue",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue // Đặt hướng là giá trị trả về
                };

                try
                {
                    string madon = GenerateId(10, "DH");
                    command.CommandText = "TaoDonHang";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Char, 10) { Value = makhach });
                    command.Parameters.Add(new SqlParameter("@StaffId", SqlDbType.Char, 10) { Value = MaNV });
                    command.Parameters.Add(new SqlParameter("@MaDon", SqlDbType.Char, 10) { Value = madon });

                    DataTable? danhSachSanPham = TaoBangThamSoSanPham(dssp);
                    if (danhSachSanPham == null)
                    {
                        throw new ArgumentException("Không có sản phẩm và số lượng tương ứng không thể tạo đơn");
                    }

                    SqlParameter tvpParam = new SqlParameter();
                    tvpParam.ParameterName = "@DanhSachSP";
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.ChiTietType";
                    tvpParam.Value = danhSachSanPham;


                    command.Parameters.Add(tvpParam);

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Donhangs.FindAsync(madon);
                    }
                    throw new Exception("Can't create DonHang");
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                    return null;
                }
                finally
                {

                }
            }
        }

        public async Task<Staff> UpdateUser(Staff users,  string Password)
        {
            await using var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await context.Staff.Include(x => x.Sysuser)
                                            .FirstOrDefaultAsync(u => u.StaffId == users.StaffId);

                if (user == null)
                {
                    throw new KeyNotFoundException("Can't Find User");
                }
                user.Avatar = users.Avatar;
                user.CuaHangId = users.CuaHangId;
                user.DiaChi = users.DiaChi;
                user.NgaySinh = users.NgaySinh;
                if (user.Sysuser != null )
                {
                    if(Password != null && !passWordService.VerifyPassword(Password,user.Sysuser.Passwords))
                    {
                        user.Sysuser.Passwords = passWordService.HashPassWord(Password);
                    }
                    
                }
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
            }
            catch (System.Exception)
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