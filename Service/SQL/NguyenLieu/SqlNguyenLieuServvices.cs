using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.NguyenLieu
{
    public class SqlNguyenLieuServices : SqLServiceBase, ISqlNguyenLieuServices
    {
        public SqlNguyenLieuServices(MyDbContext context, ILoggerFactory logger)
        : base(context, logger) { }

        public async Task<Good?> taoNguyenLieu(string tenNL, string DVT)
        {
            DbConnection connection = context.Database.GetDbConnection();

            using (DbCommand command = connection.CreateCommand())
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                var returnValueParam = new SqlParameter
                {
                    ParameterName = "@ReturnValue",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue // Đặt hướng là giá trị trả về
                };

                try
                {
                    command.CommandText = "management.usp_CreateNewGoods";
                    command.CommandType = CommandType.StoredProcedure;

                    string maNL = base.GenerateId(10,"NL");
                    command.Parameters.Add(new SqlParameter("@GoodId", SqlDbType.Char, 10) { Value = maNL });

                    command.Parameters.Add(new SqlParameter("@UnitName", SqlDbType.NVarChar, 20) { Value = DVT });

                    command.Parameters.Add(new SqlParameter("@GoodsName", SqlDbType.NVarChar, 50) { Value = tenNL });

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Goods.FindAsync(maNL);
                    }

                    throw new ArgumentException("NguyenLieu Exists in DB can't create new");
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}