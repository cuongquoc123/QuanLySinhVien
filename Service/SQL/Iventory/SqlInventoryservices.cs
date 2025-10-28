using System.Data;
using System.Data.Common;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Iventory
{
    public class SQLInventoryServices : SqLServiceBase, ISQLInventoryService
    {
        public SQLInventoryServices(MyDbContext context, ILoggerFactory logger)
        : base(context, logger) { }

        public async Task<Inventory?> softDeleteKho(string maKho)
        {
            var transaction = await context.Database.BeginTransactionAsync(); 

            try
            {
                var kho = await context.Inventories.FindAsync(maKho);

                if (kho == null)
                {
                    throw new KeyNotFoundException("Inventory does not exists");
                }
                kho.Status = "Ngưng hoạt động";

                context.Entry(kho).State = EntityState.Modified;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return kho;

            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Inventory?> taoKho(string maCH, string DiaChi)
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
                    command.CommandText = "management.usp_CreateNewInventory";
                    command.CommandType = CommandType.StoredProcedure;
                    string maKho = base.GenerateId(10, "KH");

                    command.Parameters.Add(new SqlParameter("@Makho", SqlDbType.Char, 10) { Value = maKho });
                    command.Parameters.Add(new SqlParameter("@MaCH", SqlDbType.Char, 10) { Value = maCH });
                    command.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 50) { Value = DiaChi });

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Inventories.FindAsync(maKho);
                    }
                    throw new Exception("Can't create Inventory");
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                    throw;
                }
                finally
                {
                    await dbConnection.CloseAsync();
                }



            }
        }
    }
}