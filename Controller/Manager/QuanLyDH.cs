using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/DH")]
    public class QuanLyDH : ControllerBase
    {
        private readonly MyDbContext context;
        public QuanLyDH(MyDbContext context)
        {
            this.context = context;
        }
        [HttpGet("TheoNgay/{datestart}/{dateend}/{pageNum}/{pageSize}")]
        public async Task<IActionResult> dh_theo_ngaym([FromRoute] string datestart, [FromRoute] string dateend, [FromRoute] int pageNum, [FromRoute] int pageSize)
        {
            if (string.IsNullOrEmpty(datestart) || string.IsNullOrEmpty(dateend))
            {
                throw new ArgumentException("Missing Param datestart or dateend");
            }
            if (!DateTime.TryParse(datestart, out DateTime datestarts) ||
                !DateTime.TryParse(dateend, out DateTime datends))
            {
                throw new ArgumentException("Date Format not true");
            }
            try
            {

                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userID))
                {
                    throw new UnauthorizedAccessException("Token not valid");
                }
                userID = userID.Trim();
                var user = await context.Sysusers.Include(u => u.User)
                                .FirstAsync(u => u.UserId == userID.Trim());
                if (user == null)
                {
                    throw new UnauthorizedAccessException("User not exists in Server");
                }
                
                var respone = new PageRespone<Donhang>();
                var Items = await context.Donhangs.Where(d => d.NgayNhan >= datestarts && d.NgayNhan <= datends
                                && d.User != null 
                                && d.User.User.CuaHangId == user.User.CuaHangId).
                                Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
                if (Items == null || Items.Count == 0 || !Items.Any())
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
                respone.TotalCount = await context.Donhangs.
                                        Where(d => d.NgayNhan >= datestarts && d.NgayNhan <= datends 
                                        && d.User != null 
                                         && d.User.User.CuaHangId == user.User.CuaHangId)
                                        .CountAsync();
                respone.TotalPages = (int)Math.Ceiling(respone.TotalCount / (double)10);
                respone.PageSize = pageSize;
                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw new Exception("Bad Request to Database");
            }



        }



        [HttpGet("Chitiet/{MaDon}")]
        public async Task<IActionResult> GetChitietDHm([FromRoute] string MaDon)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userID))
            {
                throw new UnauthorizedAccessException("Token not valid");
            }
            userID = userID.Trim();
            var user = await context.Sysusers.FindAsync(userID);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not exists in Server");
            }
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
