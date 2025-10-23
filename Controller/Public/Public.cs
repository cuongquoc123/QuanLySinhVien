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

        public PublicController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;

        }

        [HttpGet("Product/{pageNum}/{pageSize}")]
        public async Task<IActionResult> GetProduct([FromRoute] int pageNum, [FromRoute] int pageSize)
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
            var items = await myDbContext.Sanphams.OrderBy(x => x.TenSp )
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

                res.Items.Add(itemNew);
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

            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException("PageNum param is Out Of Range");
            }
            if (pageSize > 100 || pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("Max page size is 100");
            }
            var items = await myDbContext.Sanphams.Where(p => p.MaDm == CateId ).OrderBy(x => x.TenSp)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            if (items.Count == 0 || !items.Any() || items == null)
            {
                throw new KeyNotFoundException("Can't find any Product");
            }
            var res = new PageRespone<Sanpham>();
            foreach (var item in items)
            {
                Item<Sanpham> itemNew = new Item<Sanpham>()
                {
                    Value = item,
                    PathChiTiet = $"/public/Product/{item.MaSp}"
                };

                res.Items.Append(itemNew);
            }
            int totalCount = await myDbContext.Sanphams.Where(p => p.MaDm == CateId ).CountAsync();
            res.TotalCount = totalCount;
            res.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            res.PageIndex = pageNum;
            res.PageSize = pageSize;

            return Ok(res);
        }
        [HttpGet("ProductDetail/{masp}")]
        public async Task<IActionResult> GetChiTietProduct([FromRoute] string masp)
        {
            var respone = await myDbContext.Sanphams.FindAsync(masp);
            if (respone == null)
            {
                throw new KeyNotFoundException("Product not exists");
            }
            return Ok(respone);
        }

        [HttpGet("DanhMuc")]
        public async Task<IActionResult> getALlCate()
        {
            try
            {
                var danhmucCha = await myDbContext.LoaiDanhMucs.ToListAsync();
                if (danhmucCha == null || danhmucCha.Count == 0)
                {
                    return NoContent();
                }
                List<DanhMucRespone> danhMucRespones = new List<DanhMucRespone>();
                foreach (var cha in danhmucCha)
                {
                    var danhmucCon = await myDbContext.Danhmucs.Where(d => d.MaLoaiDm == cha.MaLoaiDm).ToListAsync();
                    List<DMRes> res = new List<DMRes>();
                    foreach (var con in danhmucCon)
                    {
                        res.Add(new DMRes()
                        {
                            tenDM = con.TenDm,
                            MaDm = con.MaDm
                        });
                    }
                    danhMucRespones.Add(new DanhMucRespone()
                    {
                        MaLoaiDm = cha.MaLoaiDm,
                        ten = cha.TenLoaiDm,
                        danhMucCon = res
                    });
                }
                return Ok(danhMucRespones);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [HttpGet("NL")]
        public async Task<IActionResult> ALLNl()
        {
            try
            {
                var respone = await myDbContext.Nguyenlieus.ToListAsync();
                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }




}