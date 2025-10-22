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

        public async Task<PhieuNhapNl?> TaoPhieuNhat(List<Product> dsNL, string Makho)
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
                    command.CommandText = "dbo.TaoPhieuNhap";
                    command.CommandType = CommandType.StoredProcedure;

                    string maphieu = base.GenerateId(10, "NL");

                    command.Parameters.Add(new SqlParameter("@MaPhieu", SqlDbType.Char, 10) { Value = maphieu });
                    command.Parameters.Add(new SqlParameter("@MaKho", SqlDbType.Char, 10) { Value = Makho });

                    SqlParameter tvpParam = new SqlParameter();
                    tvpParam.ParameterName = "@DanhSachNL";
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.ChiTietType";
                    tvpParam.Value = DanhSachNL;

                    command.Parameters.Add(tvpParam);

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value != DBNull.Value)
                    {
                        return await context.PhieuNhapNls.FindAsync(maphieu);
                    }
                    throw new ArgumentException("Mã kho bị trùng hoặc hoặc kho không tồn tại");
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                    return null;
                }
                finally
                {
                    await dbConnection.CloseAsync();
                }
            }
        }

        
    }
}