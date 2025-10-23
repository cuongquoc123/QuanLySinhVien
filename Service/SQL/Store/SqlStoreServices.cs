using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Store
{
    public class SqlStoreServices : SqLServiceBase, ISqlStoreServices
    {
        public SqlStoreServices(MyDbContext context, ILoggerFactory logger)
        : base(context, logger) { }
        public async Task<Cuahang?> CreateStore(Cuahang NewStore)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Cuahang moi = new Cuahang();
                moi.CuaHangId = GenerateId(10, "CH");
                moi.TenCh = NewStore.TenCh;
                moi.StatusS = "Đang Hoạt động";
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
                DeleteSore.StatusS = "Ngưng hoạt động";
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

    }
}