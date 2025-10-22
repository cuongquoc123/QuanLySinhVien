using System.Security.Claims;
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
    [Route("manager/Kho")]
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

        [HttpGet("{pageNum}/{pageSize}")]
        public async Task<IActionResult> GetALlKho([FromRoute] int pageNum, [FromRoute] int pageSize)
        {
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException("PageNum param is Out Of Range");
            }
            if (pageSize > 100 || pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("Max page size is 100");
            }
            var ManagerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(ManagerId))
            {
                throw new UnauthorizedAccessException("Token not valid");
            }
            var Manager = await context.Sysusers.Include(u => u.User).FirstAsync(u => u.UserId == ManagerId);

            if (Manager == null)
            {
                throw new CustomError(403, "Forbiden", "You do not exists in my DB");
            }


            var items = await context.Khos.Where(p => p.CuaHangId == Manager.User.CuaHangId).OrderBy(p => p.MaKho)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            if (items.Count == 0 || !items.Any() || items == null)
            {
                throw new KeyNotFoundException("Can't find any Product");
            }
            var res = new PageRespone<Kho>();
            foreach (var item in items)
            {
                Item<Kho> itemNew = new Item<Kho>()
                {
                    Value = item,
                    PathChiTiet = $"/manager/Kho/Detail/{item.MaKho}"
                };

                res.Items.Append(itemNew);
            }
            int totalCount = await context.Khos.Where(p => p.CuaHangId == Manager.User.CuaHangId)
                                .CountAsync();
            res.TotalCount = totalCount;
            res.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            res.PageIndex = pageNum;
            res.PageSize = pageSize;

            return Ok(res);
        }

        [HttpGet("Detail/{MaKho}")]
        public async Task<IActionResult> GetDetailKho([FromRoute] string MaKho)
        {
            if (string.IsNullOrEmpty(MaKho))
            {
                throw new ArgumentNullException("Missing Param MaKho");
            }
            var respone = await context.TonKhos.Where(TK => TK.MaKho == MaKho).ToListAsync();
            var Khos = await context.Khos.FindAsync(MaKho);

            return Ok(new
            {
                Kho = Khos,
                TonKho = respone
            });
        }

        [HttpPost("NhapKho/{MaKho}")]
        public async Task<IActionResult> TaoPhieuNhapNL([FromRoute] string MaKho, [FromBody] List<Product> dsNL)
        {
            if (string.IsNullOrEmpty(MaKho))
            {
                throw new ArgumentException("Missing param 'MaKho' on route");
            }
            try
            {
                var respone = await sqlPhieuNhapKho.TaoPhieuNhat(Makho: MaKho, dsNL: dsNL);

                return Ok(respone);
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

            var Manager = await context.Staff.FindAsync(ManagerId);

            if (Manager == null)
            {
                throw new CustomError(403, "Forbiden", "This user Does not exists in my DB");
            }
            if (string.IsNullOrEmpty(Manager.CuaHangId))
            {
                throw new CustomError(403, "Forbiden", "This user don't have permission to create Kho");
            }
            try
            {
                var respone = await sQLInventoryService.taoKho(Manager.CuaHangId, DiaChi);

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
        public async Task<IActionResult> SoftDeleteKho ([FromRoute] string Makho)
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
                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}