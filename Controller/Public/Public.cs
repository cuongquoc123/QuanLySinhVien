using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Public
{
    [ApiController]
    [Route("/public")]
    public class PublicController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        private readonly ISqLServices sqLServices;
        public PublicController(MyDbContext myDbContext, ISqLServices sqLServices)
        {
            this.myDbContext = myDbContext;
            this.sqLServices = sqLServices;
        }

        [HttpGet("Product")]
        public async Task<IActionResult> GetProduct([FromQuery] int pageNum, [FromQuery] int pageSize)
        {
            var res = new PageRespone<Sanpham>();
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException("PageNum param is Out Of Range");
            }
            if (pageSize > 100 || pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("Max page size is 100");
            }
            var items = await myDbContext.Sanphams.OrderBy(x => x.TenSp)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            
            if (items == null || items.Count == 0 || !items.Any())
            {
                return NoContent();
            }
            foreach (var item in items)
            {
                Item<Sanpham> itemNew = new Item<Sanpham>()
                {
                    Value = item,
                    PathChiTiet = $"/ChiTiet/{item.MaSp}"
                };

                res.Items.Append(itemNew);
            }
            int totalCount = await myDbContext.Sanphams.CountAsync();
            res.TotalCount = totalCount;
            res.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            res.PageIndex = pageNum;
            res.PageSize = pageSize;
            return Ok(res);
        }

        [HttpGet("ProductByCate/{pageNum}/{pageSize}/{CateId}")]
        public async Task<IActionResult> GetProduct([FromRoute] int pageNum, [FromRoute] int pageSize, [FromRoute] string CateId)
        {
            var res = new PageRespone<Sanpham>();
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException("PageNum param is Out Of Range");
            }
            if (pageSize > 100 || pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("Max page size is 100");
            }
            var items = await myDbContext.Sanphams.Where(p => p.MaDm == CateId).OrderBy(x => x.TenSp)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();

            foreach (var item in items)
            {
                Item<Sanpham> itemNew = new Item<Sanpham>()
                {
                    Value = item,
                    PathChiTiet = $"/public/Product/ChiTiet/{item.MaSp}"
                };

                res.Items.Append(itemNew);
            }
            int totalCount = await myDbContext.Sanphams.Where(p => p.MaDm == CateId).CountAsync();
            res.TotalCount = totalCount;
            res.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            res.PageIndex = pageNum;
            res.PageSize = pageSize;

            return Ok(res);
        }
        [HttpGet("/Chitiet/{masp}")]
        public async Task<IActionResult> GetChiTietProduct([FromRoute] string masp)
        {
            var respone = await myDbContext.Sanphams.FindAsync(masp);
            return Ok(respone);
        }
        

    }
    

}