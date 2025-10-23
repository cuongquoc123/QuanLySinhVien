using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Controller.Cashier;
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
            //Chỉ định format của ngày tháng 
            string expectedFormat = "dd-MM-yyyy";
           

            var culture = System.Globalization.CultureInfo.InvariantCulture;
            if (!DateTime.TryParseExact(datestart,expectedFormat,culture,System.Globalization.DateTimeStyles.None, out DateTime datestarts) ||
                !DateTime.TryParseExact(dateend,expectedFormat,culture,System.Globalization.DateTimeStyles.None, out DateTime datends))
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
                var user = await context.Staff
                                .FirstAsync(u => u.StaffId == userID.Trim());
                if (user == null)
                {
                    throw new UnauthorizedAccessException("User not exists in Server");
                }

                var respone = new PageRespone2();
                var Items = await context.Donhangs.Where(d => d.NgayNhan >= datestarts && d.NgayNhan <= datends
                                && d.User != null
                                && d.User.User.CuaHangId == user.CuaHangId).
                                Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
                if (Items == null || Items.Count == 0 || !Items.Any())
                {
                    return NoContent();
                }

                foreach (var item in Items)
                {
                    Item2 itemNew = new Item2()
                    {
                        maDon = item.MaDon,
                        TrangTHai = item.TrangThai,
                        NgayNhan = item.NgayNhan,
                        NgayHoangThanh = item.NgayHoangThanh,
                        UserId = item.UserId,
                        PathChiTiet = $"/manager/DH/chitiet/{item.MaDon}"
                    };
                    var CTDH = await context.ChiTietDonHangs.Include(d => d.MaSpNavigation).Where(d => d.MaDon == item.MaDon).ToListAsync();
                    
                    foreach (var item2 in CTDH)
                    {
                        CTDHRespone CTDHS = new CTDHRespone()
                        {
                            masp = item2.MaSp,
                            tenSP = item2.MaSpNavigation.TenSp,
                            SoLuong = item2.SoLuong,
                            Gia = item2.MaSpNavigation.DonGia,
                            ThanhTiem = item2.SoLuong * item2.MaSpNavigation.DonGia
                        };
                        itemNew.CTDH.Add(CTDHS);
                    }
                    
                    
                    respone.Items.Add(itemNew);
                }
                respone.PageIndex = pageNum;
                respone.TotalCount = await context.Donhangs.
                                        Where(d => d.NgayNhan >= datestarts && d.NgayNhan <= datends
                                        && d.User != null
                                         && d.User.User.CuaHangId == user.CuaHangId)
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

            var chiTiets = await context.ChiTietDonHangs.Include(ct => ct.MaSpNavigation).Where(d => d.MaDon == MaDon).ToListAsync();
            List<CTDHRespone> CTDHS = new List<CTDHRespone>();
            decimal TongCong = 0;
            foreach (var CT in chiTiets)
            {
                CTDHRespone CTDH = new CTDHRespone()
                        {
                            masp = CT.MaSp,
                            tenSP = CT.MaSpNavigation.TenSp,
                            SoLuong = CT.SoLuong,
                            Gia = CT.MaSpNavigation.DonGia,
                            ThanhTiem = CT.SoLuong * CT.MaSpNavigation.DonGia
                        };
                CTDHS.Add(CTDH);
                TongCong += CTDH.ThanhTiem;
            }
            return Ok(new
            {
                MaDon = DonHang.MaDon,
                User = DonHang.UserId,
                NgayNhan = DonHang.NgayNhan,
                NgayHoangThanh = DonHang.NgayHoangThanh,
                customer = DonHang.CustomerId,
                TrangThai = DonHang.TrangThai,
                ThanhTien = TongCong * (decimal)1.1,
                TaxVat = "10%",
                chi_tiet = CTDHS
            });
        }
    }
}
