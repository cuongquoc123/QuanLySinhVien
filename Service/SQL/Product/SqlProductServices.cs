using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.Service.SQL.ProductS
{
    public class SqlProductServiecs : SqLServiceBase, ISqlProductServiecs
    {

        public SqlProductServiecs(MyDbContext context, ILoggerFactory logger) : base(context, logger) { }
        
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

    }
}