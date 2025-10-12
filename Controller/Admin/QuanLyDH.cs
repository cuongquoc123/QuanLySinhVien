using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("/admin/DH")]
    public class QuanLyDH : ControllerBase
    {
        private readonly MyDbContext context; 
        public QuanLyDH(MyDbContext context)
        {
            this.context = context;
        }
        [HttpGet("TheoNgay/{datestart}/{dateend}")]
        public async Task<IActionResult> dh_theo_ngay([FromRoute] string datestart, [FromRoute] string dateend, [FromRoute] int pageNum, [FromRoute] int pageSize)
        {
            if (string.IsNullOrEmpty(datestart) || string.IsNullOrEmpty(dateend))
            {
                throw new ArgumentException("Missing Param datestart or dateend");
            }
            DateOnly datestarts = DateOnly.Parse(datestart);
            DateOnly datends = DateOnly.Parse(dateend);

            var respone = new PageRespone<Donhang>();

            var Items = await context.Donhangs.Where(d => d.NgayNhan > datestarts && d.NgayNhan < datends).
                                Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();

            if(Items == null || Items.Count == 0 || !Items.Any())
            {
                return NoContent();
            }
            
            foreach (var item in Items)
            {
                Item<Donhang> itemNew = new Item<Donhang>()
                {
                    Value = item,
                    PathChiTiet = $"/admin/DH/chitiet/{item.MaDon}"
                };
                respone.Items.Append(itemNew);
            }
            respone.PageIndex = pageNum;
            respone.TotalCount = await context.Donhangs.Where(d => d.NgayNhan > datestarts && d.NgayNhan < datends).CountAsync();
            respone.TotalPages = (int)Math.Ceiling(respone.TotalCount / (double)10);
            respone.PageSize = pageSize;
            return Ok(respone);
        }
        
        

        [HttpGet("Chitiet/{MaDon}")]
        public async Task<IActionResult> GetChitietDH([FromRoute] string MaDon)
        {
            var DonHang = await context.Donhangs.FindAsync(MaDon);
            if (DonHang == null)
            {
                throw new KeyNotFoundException("The bill doesn't exists");
            }
            var chiTiets = await context.ChiTietDonHangs.Where(d => d.MaDon == MaDon).ToListAsync();
            return Ok(new
            {
                donHang = DonHang,
                chi_tiet = chiTiets
            });
        }
    }
}