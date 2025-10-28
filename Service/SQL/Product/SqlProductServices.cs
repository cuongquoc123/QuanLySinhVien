using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.Service.SQL.ProductS
{
    public class SqlProductServiecs : SqLServiceBase, ISqlProductServiecs
    {

        public SqlProductServiecs(MyDbContext context, ILoggerFactory logger) : base(context, logger) { }
        
        public async Task<Product?> CreateProDucts(Product spMoi, string imgPath)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {

                Product moi = new Product();
                moi.ProductId = GenerateId(10, "SP");
                moi.ProductName = spMoi.ProductName;
                moi.Img = imgPath;
                moi.Price = spMoi.Price;
                moi.SubcategoryId = spMoi.SubcategoryId;
                moi.Decription = spMoi.Decription;
                await context.Products.AddAsync(moi);
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
                Product? sp = await context.Products.FindAsync(productId);
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