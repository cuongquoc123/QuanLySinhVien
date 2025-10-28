using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.Order
{
    public class OrderService : SqLServiceBase, IOrderService
    {
        public OrderService (MyDbContext context, ILoggerFactory logger)
        : base (context, logger) {}

        public async Task<Models.Order?> updateDonStatus(string madon, string status)
        {
            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Models.Order? UpdateDon = await context.Orders.FindAsync(madon);
                if (UpdateDon == null)
                {
                    throw new KeyNotFoundException("Don Hang not exists");
                }
                UpdateDon.Status = status;
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

        public async Task<Models.Order?> taoDon( int MaNV, List<ProductItem> dssp, string makhach)
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
                    command.CommandText = "dbo.usp_CreateOrder";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Char, 10) { Value = makhach });
                    command.Parameters.Add(new SqlParameter("@StaffId", SqlDbType.Int) { Value = MaNV });
                    command.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Char, 10) { Value = madon });

                    DataTable? danhSachSanPham = TaoBangThamSoSanPham(dssp);
                    if (danhSachSanPham == null)
                    {
                        throw new ArgumentException("Không có sản phẩm và số lượng tương ứng không thể tạo đơn");
                    }

                    SqlParameter tvpParam = new SqlParameter();
                    tvpParam.ParameterName = "@ListProduct";
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.DetailType";
                    tvpParam.Value = danhSachSanPham;


                    command.Parameters.Add(tvpParam);

                    await command.ExecuteNonQueryAsync();

                    if (returnValueParam.Value == null )
                    {
                        var donhang = await context.Orders.Include(ord => ord.OrderDetails).ThenInclude(detail => detail.Product).FirstOrDefaultAsync(dh => dh.OrderId == madon);
                        
                        return donhang;
                    }
                    throw new Exception("Can't create DonHang");
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
