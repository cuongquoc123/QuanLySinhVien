using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/Staff")]
    public class QuanLyStaff : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqLServices sqLServices;
        public QuanLyStaff(MyDbContext context, ISqLServices sqLServices)
        {
            this.context = context;
            this.sqLServices = sqLServices;
        }



        [HttpGet("/{pageNum}/{pageSize}")]
        public async Task<IActionResult> GetStaff([FromRoute] int pageNum, [FromRoute] int pageSize)
        {

            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException("PageNum param is Out Of Range");
            }
            if (pageSize > 100 || pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("Max page size is 100");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new KeyNotFoundException("Missing Token");
            }
            var user = await context.Sysusers.FindAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            var lsStaff = await context.Staff.Where(s => s.CuaHangId == user.CuaHangId)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            if (lsStaff == null || lsStaff.Count == 0 || !lsStaff.Any())
            {
                throw new KeyNotFoundException("Can't find any Staff");
            }
            var respone = new PageRespone<Staff>();

            foreach (var s in lsStaff)
            {
                Item<Staff> item = new Item<Staff>()
                {
                    Value = s,
                    PathChiTiet = $"/manager/Staff/{s.Cccd}"
                };
                respone.Items.Append(item);
            }

            int total = await context.Staff.Where(s => s.CuaHangId == user.CuaHangId).CountAsync();
            respone.TotalCount = total;
            respone.PageIndex = pageNum;
            respone.PageSize = pageSize;
            respone.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            return Ok(respone);
        }
        [HttpGet("/{StaffId}")]
        public async Task<IActionResult> StaffDetail([FromRoute] string StaffId)
        {
            if (string.IsNullOrEmpty(StaffId))
            {
                throw new ArgumentNullException("Missing Param StaffId");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new KeyNotFoundException("Missing Token");
            }
            var user = await context.Sysusers.FindAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }

            Staff respone = await context.Staff
                            .FirstAsync(s => s.Cccd == StaffId && s.CuaHangId == user.CuaHangId);

            if (respone == null)
            {
                throw new KeyNotFoundException("Your Store do not have this staff or you don't have permision this staff");
            }
            return Ok(respone);
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateStaff ([FromBody])
        // {
            
        // }
    }
}