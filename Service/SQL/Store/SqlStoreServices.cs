using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Store
{
    public class SqlStoreServices : SqLServiceBase, ISqlStoreServices
    {
        public SqlStoreServices(MyDbContext context, ILoggerFactory logger)
        : base(context, logger) { }
        public async Task<Models.Store?> CreateStore(Models.Store NewStore)
        {
            var Transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Models.Store moi = new Models.Store();
                moi.StoreId = GenerateId(10, "CH");
                moi.StoreName = NewStore.StoreName;
                moi.StoreAddr = NewStore.StoreAddr;
                moi.PhoneNum = NewStore.PhoneNum;
                moi.StoreStatus = "Hoạt Động";
                await context.Stores.AddAsync(moi);
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
                Models.Store? DeleteSore = await context.Stores.FindAsync(StoreId);
                if (DeleteSore == null)
                {
                    return 404;
                }
                DeleteSore.StoreStatus = "Ngưng hoạt động";
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