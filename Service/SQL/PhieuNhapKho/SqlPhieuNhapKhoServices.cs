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

        public async Task<Gon?> CreateOrderGoods(List<ProductItem> dsNL, int InventoryId)
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

                    command.CommandText = "management.usp_CreateGON";
                    command.CommandType = CommandType.StoredProcedure;

                    string GonId = base.GenerateId(10, "GON");

                    command.Parameters.Add(new SqlParameter("@GON_ID", SqlDbType.Char, 10) { Value = GonId });
                    command.Parameters.Add(new SqlParameter("@inventoryId", SqlDbType.Int) { Value = InventoryId });

                    SqlParameter tvpParam = new SqlParameter();
                    tvpParam.ParameterName = "@ListGoods";
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.DetailType";
                    tvpParam.Value = ListGoods;

                    command.Parameters.Add(tvpParam);

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Gons.Include(gon => gon.Gondetails).ThenInclude(detail => detail.Good).FirstAsync(grn => grn.Gonid ==  GonId);
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

        public async Task<Grn?> TaoPhieuNhat(List<ProductItem> dsNL, int Makho)
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
                    var DanhSachNL = base.TaoBangThamSoSanPham(dsNL);
                    if (DanhSachNL == null)
                    {
                        throw new ArgumentException("Need List NguyenLieu to create Bill");
                    }
                    command.CommandText = "management.usp_CreateGRN";
                    command.CommandType = CommandType.StoredProcedure;

                    string maphieu = base.GenerateId(10, "PH");

                    command.Parameters.Add(new SqlParameter("@GRN_ID", SqlDbType.Char, 10) { Value = maphieu });
                    command.Parameters.Add(new SqlParameter("@inventoryId", SqlDbType.Int) { Value = Makho });

                    SqlParameter tvpParam = new SqlParameter();
                    tvpParam.ParameterName = "@ListGoods";
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.DetailType";
                    tvpParam.Value = DanhSachNL;

                    command.Parameters.Add(tvpParam);

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.Grns.Include(grn => grn.Grndetails).ThenInclude(detail => detail.Good).FirstAsync(grn => grn.GrnId ==  maphieu);
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