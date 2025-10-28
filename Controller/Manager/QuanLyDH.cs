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
                var user = await context.Sysusers.FindAsync(int.Parse(userID));
                if (user == null)
                {
                    throw new UnauthorizedAccessException("User not exists in Server");
                }

                var respone = new PageRespone2();
                var Items = await context.Orders.Include(ord => ord.SysUser).Include(ord => ord.OrderDetails)
                                .ThenInclude(detail => detail.Product)
                                .Where(d => d.RecivingDate >= datestarts && d.RecivingDate <= datends
                                && d.SysUser != null
                                && d.SysUser.StoreId == user.StoreId).
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
                        maDon = item.OrderId,
                        TrangTHai = item.Status,
                        NgayNhan = item.RecivingDate,
                        NgayHoangThanh = item.CompleteDate,
                        User = item.SysUser?.UserName,
                        PathChiTiet = $"/manager/DH/chitiet/{item.OrderId}"
                    };
                    
                    foreach (var item2 in item.OrderDetails)
                    {
                        CTDHRespone CTDHS = new CTDHRespone()
                        {
                            masp = item2.ProductId,
                            tenSP = item2.Product.ProductName,
                            SoLuong = item2.Quantity,
                            Gia = item2.Product.Price,
                            ThanhTiem = item2.Quantity * item2.Product.Price
                        };
                        itemNew.CTDH.Add(CTDHS);
                    }
                    
                    
                    respone.Items.Add(itemNew);
                }
                respone.PageIndex = pageNum;
                respone.TotalCount = await context.Orders.
                                        Where(d => d.RecivingDate >= datestarts && d.RecivingDate <= datends
                                        && d.SysUser != null
                                         && d.SysUser.StoreId == user.StoreId)
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
            var DonHang = await context.Orders.Include(ord => ord.SysUser).Include(ord => ord.OrderDetails)
                        .ThenInclude(detail => detail.Product)
                        .FirstAsync(ord => ord.OrderId == MaDon);
            if (DonHang == null)
            {
                throw new KeyNotFoundException("The bill doesn't exists");
            }
            List<CTDHRespone> CTDHS = new List<CTDHRespone>();
            decimal TongCong = 0;
            foreach (var CT in DonHang.OrderDetails)
            {
                CTDHRespone CTDH = new CTDHRespone()
                {
                    masp = CT.ProductId,
                    tenSP = CT.Product.ProductName,
                    SoLuong = CT.Quantity,
                    Gia = CT.Product.Price,
                    ThanhTiem = CT.Quantity * CT.Product.Price
                };
                CTDHS.Add(CTDH);
                TongCong += CTDH.ThanhTiem;
            }
            return Ok(new
            {
                MaDon = DonHang.OrderId,
                User = DonHang.SysUser?.UserName,
                NgayNhan = DonHang.RecivingDate,
                NgayHoangThanh = DonHang.CompleteDate,
                customer = DonHang.CustomerId,
                TrangThai = DonHang.Status,
                ThanhTien = TongCong * (decimal)1.1,
                TaxVat = "10%",
                chi_tiet = CTDHS
            });
        }

        
    }
}
