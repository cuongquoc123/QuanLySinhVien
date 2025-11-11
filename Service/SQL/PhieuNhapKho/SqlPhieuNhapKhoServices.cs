using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;
using Serilog;

namespace QuanLySinhVien.Service.SQL.PhieuNhapKho
{
    public class SqlPhieuNhapKhoServices : SqLServiceBase, ISqlPhieuNhapKho
    {
        public SqlPhieuNhapKhoServices(MyDbContext context, ILoggerFactory logger)
        : base(context, logger) { }

        public async Task<Inventoryrecord?> CreateInventoryRecords(List<ProductItem> dsNL, int Inventory,int RecordsType)
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
                    var ListGoods = base.TaoBangThamSoSanPham(dsNL);
                    if (ListGoods == null)
                    {
                        throw new ArgumentException("Need List Good to create Order");
                    }

                    command.CommandText = "management.usp_CreateInventoryRecords";
                    command.CommandType = CommandType.StoredProcedure;

                    string RecordsId = base.GenerateId(10, "PH");

                    command.Parameters.Add(new SqlParameter("@InventoryIdRecordId", SqlDbType.Char, 10) { Value = RecordsId });
                    command.Parameters.Add(new SqlParameter("@inventoryId", SqlDbType.Int) { Value = Inventory });
                    command.Parameters.Add(new SqlParameter("@TypeID", SqlDbType.Int) { Value = RecordsType });


                    SqlParameter tvpParam = new SqlParameter();
                    tvpParam.ParameterName = "@ListGoods";
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.DetailType";
                    tvpParam.Value = ListGoods;

                    command.Parameters.Add(tvpParam);

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Inventoryrecords.Include(x => x.RecorDetails).ThenInclude(x => x.Good).FirstOrDefaultAsync(x => x.RecordsId == RecordsId);
                    }
                    throw new ArgumentException("Mã kho bị trùng hoặc hoặc kho không tồn tại");
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