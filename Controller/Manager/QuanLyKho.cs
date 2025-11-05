using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL.Iventory;
using QuanLySinhVien.Service.SQL.PhieuNhapKho;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("manager/kho")]
    [Authorize(Roles = "Admin,Manager")]
    public class QuanLyKho : ControllerBase
    {
        private readonly ISQLInventoryService sQLInventoryService;
        private readonly ISqlPhieuNhapKho sqlPhieuNhapKho;
        private readonly MyDbContext context;
        public QuanLyKho(ISqlPhieuNhapKho sqlPhieuNhapKho, MyDbContext context, ISQLInventoryService sQLInventoryService)
        {
            this.context = context;
            this.sqlPhieuNhapKho = sqlPhieuNhapKho;
            this.sQLInventoryService = sQLInventoryService;
        }


        [HttpGet("Detail")]
        public async Task<IActionResult> GetDetailKho()
        {
            var managerid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerid == null)
            {
                throw new UnauthorizedAccessException("Token not valid");
            }
            var manager = context.Sysusers.Find(int.Parse(managerid));

            if (manager == null)
            {
                throw new UnauthorizedAccessException("Fake token");
            }
            var TonKho = await context.Stocks.Include(Stk => Stk.Inventory)
                        .Include(Stk => Stk.Good).Where(stk =>stk.Inventory != null && stk.Inventory.StoreId == manager.StoreId ).ToListAsync();
            List<TonKhoRespone> tonKhoRespones = new List<TonKhoRespone>();
            int? Khoid = 0;
            foreach (var item in TonKho)
            {
                tonKhoRespones.Add(new TonKhoRespone()
                {
                    GoodId = item.GoodId,
                    GoodName = item.Good.GoodName,
                    UnitName = item.Good.UnitName,
                    InStock = item.InStock,
                    Status = item.Status
                });
                Khoid = item.InventoryId;
            }
            return Ok(new
            {
                InventoryId = Khoid,
                Stock = tonKhoRespones
            });
        }

        [HttpPost("NhapKho/{MaKho}")]
        public async Task<IActionResult> TaoPhieuNhapNL([FromRoute] string MaKho, [FromBody] List<ProductItem> dsNL)
        {
            if (string.IsNullOrEmpty(MaKho))
            {
                throw new ArgumentException("Missing param 'MaKho' on route");
            }
            try
            {
                var respone = await sqlPhieuNhapKho.TaoPhieuNhat(Makho: MaKho, dsNL: dsNL);
                if (respone == null)
                {
                    throw new Exception("Can't create PhieuNhat");
                }

                List<TonKhoRespone> tonKhoRespones = new List<TonKhoRespone>();
                foreach (var item in respone.Grndetails)
                {
                    tonKhoRespones.Add(new TonKhoRespone()
                    {
                        GoodId = item.Good.GoodId,
                        GoodName = item.Good.GoodName,
                        UnitName = item.Good.UnitName,
                        InStock = item.ReStock,
                    });
                }
                return Ok(new
                {
                    GRNId = respone.GrnId,
                    InventoryId = respone.InventoryId,
                    AdmissionDate = respone.AdmissionDate,
                    Detail = tonKhoRespones
                });
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        [HttpPost()]
        public async Task<IActionResult> TaoKho([FromQuery] string DiaChi)
        {
            if (string.IsNullOrWhiteSpace(DiaChi))
            {
                throw new ArgumentException("Missing Param 'DiaChi' on query");
            }
            var ManagerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ManagerId))
            {
                throw new UnauthorizedAccessException("Token Not valid");
            }

            var Manager = await context.Sysusers.FindAsync(int.Parse(ManagerId));

            if (Manager == null)
            {
                throw new CustomError(403, "Forbiden", "This user Does not exists in my DB");
            }
            if (string.IsNullOrEmpty(Manager.StoreId))
            {
                throw new CustomError(403, "Forbiden", "This user don't have permission to create Kho");
            }
            try
            {
                var respone = await sQLInventoryService.taoKho(Manager.StoreId, DiaChi);

                if (respone == null)
                {
                    throw new Exception("Can't create Kho");
                }

                return Ok(respone);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [HttpPut("{Makho}")]
        public async Task<IActionResult> SoftDeleteKho([FromRoute] string Makho)
        {
            if (string.IsNullOrEmpty(Makho))
            {
                throw new ArgumentException("missing param 'Makho' on route");
            }
            try
            {
                var respone = await sQLInventoryService.softDeleteKho(Makho);
                if (respone == null)
                {
                    throw new Exception("Can't delete Inventory");
                }
                return Ok(new
                {
                    message = "Delete SuccesFull"
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}