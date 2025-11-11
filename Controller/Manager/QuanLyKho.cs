using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL.PhieuNhapKho;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("manager/kho")]
    [Authorize(Roles = "Admin,Manager")]
    public class QuanLyKho : ControllerBase
    {

        private readonly ISqlPhieuNhapKho sqlPhieuNhapKho;
        private readonly MyDbContext context;
        public QuanLyKho(ISqlPhieuNhapKho sqlPhieuNhapKho, MyDbContext context)
        {
            this.context = context;
            this.sqlPhieuNhapKho = sqlPhieuNhapKho;

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
                        .Include(Stk => Stk.Good).Where(stk => stk.Inventory != null && stk.Inventory.StoreId == manager.StoreId).ToListAsync();
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

        [HttpPost("Stock/{InventoryId}/{TypeId}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int InventoryId, [FromBody] List<ProductItem> requests, [FromRoute] int TypeId)
        {
            try
            {
                var res = await sqlPhieuNhapKho.CreateInventoryRecords(requests, InventoryId, TypeId);
                if (res != null)
                {
                    List<TonKhoRespone> tonKhoRespones = new List<TonKhoRespone>();
                    foreach (var item in res.RecorDetails)
                    {
                        tonKhoRespones.Add(new TonKhoRespone()
                        {
                            GoodId = item.Good.GoodId,
                            GoodName = item.Good.GoodName,
                            UnitName = item.Good.UnitName,
                            InStock = item.Quantity,
                        });
                    }
                    return Ok(new
                    {
                        RecordId = res.RecordsId,
                        InventoryId = res.InventoryId,
                        AdmissionDate = res.AdmissionDate,
                        Detail = tonKhoRespones
                    });
                }
                throw new Exception("Can't update Stock in Database");
            }
            catch (KeyNotFoundException)
            {

                throw;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("Record")]
        public async Task<IActionResult> GetRecord([FromQuery] int PageNum, [FromQuery] int PageSize)
        {
            try
            {
                var InventoryRecords = await context.Inventoryrecords.Include(r => r.Type)
                                            .Skip((PageNum - 1) * PageSize).Take(PageSize)
                                            .OrderBy(x => x.AdmissionDate).ToListAsync();

                List<Item<RecordsInventoryRespone>> ResponeItems = new List<Item<RecordsInventoryRespone>>();

                foreach (var item in InventoryRecords)
                {
                    var Record = new RecordsInventoryRespone()
                    {
                        RecordId = item.RecordsId,
                        AdmissionDate = item.AdmissionDate,
                        TypeId = item.TypeId,
                        TypeName = item.Type.TypeName
                    };
                    ResponeItems.Add(new Item<RecordsInventoryRespone>()
                    {
                        Value = Record,
                        PathChiTiet = $"Record/{item.RecordsId}"
                    });
                }

                var Res = new PageRespone<RecordsInventoryRespone>();
                Res.Items = ResponeItems;

                Res.TotalCount = await context.Inventoryrecords.CountAsync();
                Res.TotalPages = (int)Math.Ceiling(Res.TotalCount / (double)PageSize);
                Res.PageIndex = PageNum;
                Res.PageSize = PageSize;
                return Ok(Res);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("Record/{RecordId}")]
        public async Task<IActionResult> RecordDetail([FromRoute] string RecordId)
        {
            try
            {
                var res = await context.Inventoryrecords.Include(x => x.RecorDetails).
                                ThenInclude(x => x.Good).FirstOrDefaultAsync(x => x.RecordsId == RecordId);
                if (res != null)
                {
                    List<TonKhoRespone> tonKhoRespones = new List<TonKhoRespone>();
                    foreach (var item in res.RecorDetails)
                    {
                        tonKhoRespones.Add(new TonKhoRespone()
                        {
                            GoodId = item.Good.GoodId,
                            GoodName = item.Good.GoodName,
                            UnitName = item.Good.UnitName,
                            InStock = item.Quantity,
                        });
                    }
                    return Ok(new
                    {
                        RecordId = res.RecordsId,
                        InventoryId = res.InventoryId,
                        AdmissionDate = res.AdmissionDate,
                        Detail = tonKhoRespones
                    });
                }
                throw new Exception("Can't update Stock in Database");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("Record/Search")]
        public async Task<IActionResult> GetRecords([FromQuery] DateTime DateStart, [FromQuery] DateTime DateEnd, [FromQuery] int TypeId)
        {
            DateStart = DateStart.AddDays(-1);
            DateEnd = DateEnd.AddDays(1);

            try
            {
                List<Inventoryrecord> Res;
                if (TypeId == 0)
                {
                    Res = await context.Inventoryrecords.Include(x => x.Type).OrderBy(x => x.AdmissionDate)
                                        .Where(x => x.AdmissionDate > DateStart && x.AdmissionDate < DateEnd)
                                        .ToListAsync();
                }
                else
                {
                    Res = await context.Inventoryrecords.Include(x => x.Type).OrderBy(x => x.AdmissionDate)
                                        .Where(x => x.TypeId == TypeId && x.AdmissionDate > DateStart && x.AdmissionDate < DateEnd)
                                        .ToListAsync();
                }

                if (Res == null)
                {
                    return NoContent();
                }
                List<Item<RecordsInventoryRespone>> ResponeItems = new List<Item<RecordsInventoryRespone>>();

                foreach (var item in Res)
                {
                    var Record = new RecordsInventoryRespone()
                    {
                        RecordId = item.RecordsId,
                        AdmissionDate = item.AdmissionDate,
                        TypeId = item.TypeId,
                        TypeName = item.Type.TypeName
                    };
                    ResponeItems.Add(new Item<RecordsInventoryRespone>()
                    {
                        Value = Record,
                        PathChiTiet = $"Record/{item.RecordsId}"
                    });
                }

                return Ok(ResponeItems);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("TypeRecord")]
        public async Task<IActionResult> GetAllTypeRecord()
        {
            try
            {
                var types = await context.Recordtypes.OrderBy(x => x.TypeId).ToListAsync();
                if (types != null)
                {
                    var res = new List<object>();
                    res.Add(new
                    {
                        TypeId = 0,
                        TypeName = "All type"
                    });
                    foreach (var type in types)
                    {
                        res.Add(new
                        {
                            TypeId = type.TypeId,
                            TypeName = type.TypeName
                        });
                    }
                    return Ok(res);
                }
                throw new Exception("Can't get records type in db ");
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}