using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Public
{
    [ApiController]
    [Route("")]
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
        public async Task<IActionResult> GetProduct([FromQuery]int pageNum,[FromQuery] int pageSize)
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
            res.Items = await myDbContext.Sanphams.OrderBy(x => x.TenSp)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            int totalCount = await myDbContext.Sanphams.CountAsync();
            res.TotalCount = totalCount;
            res.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            res.PageIndex = pageNum;
            res.PageSize = pageSize;

            return Ok(res);
        }
    }
}