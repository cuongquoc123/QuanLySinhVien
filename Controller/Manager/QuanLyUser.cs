using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/user")]
    public class QuanlyUser: ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqLServices sqLServices;

        public QuanlyUser(MyDbContext context, ISqLServices sqLServices)
        {
            this.context = context;
            this.sqLServices = sqLServices;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUserm([FromBody]CreateUserRequest req)
        {
            if (string.IsNullOrEmpty(req.UserName) || string.IsNullOrEmpty(req.Password)
             || string.IsNullOrEmpty(req.RoleId))
            {
                throw new ArgumentException("Missing Param UserName, Password or RoleId");
            }
            var USID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (USID == null)
            {
                throw new KeyNotFoundException("Not Found User In Token");
            }
            var Manager = await context.Sysusers.FindAsync(USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            Sysuser user = new Sysuser()
            {
                UserName = req.UserName,
                Passwords = req.Password,
                RoleId = req.RoleId,
                CuaHangId = Manager.CuaHangId,
            };
            try
            {
                var respone = await sqLServices.CreateUser(user);
                if (respone == null)
                {
                    throw new Exception("User can't be create");
                }

                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateUserm([FromBody]Sysuser req)
        {
            var USID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (USID == null)
            {
                throw new KeyNotFoundException("Not Found User In Token");
            }
            var Manager = await context.Sysusers.FindAsync(USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            if (req.CuaHangId != Manager.CuaHangId)
            {
                throw new CustomError(403, "Not Permission", "Can't modify User");
            }
            if (ModelState.IsValid)
            {
                var user = await sqLServices.UpdateUser(req);
                return Ok(user);
            }
            throw new CustomError(400, "Bad Request", "Can't Update User Info");
        }

        [HttpPut("DUser/{req}")]
        public async Task<IActionResult> DeleteUserm([FromRoute] string req)
        {
            var USID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (USID == null)
            {
                throw new KeyNotFoundException("Not Found User In Token");
            }
            var Manager = await context.Sysusers.FindAsync(USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            var user = await context.Sysusers.FindAsync(req);
            if (user == null)
            {
                throw new KeyNotFoundException("User not exists in Database => can't delete");
            }
            if (user.CuaHangId != Manager.CuaHangId)
            {
                throw new CustomError(403, "Not Permission", "Can't modify User");
            }
            if (await sqLServices.SoftDeleteUser(req) == 200)
            {
                return Ok(new
                {
                    Message = "Delete Succesfull",
                });
            }
            throw new Exception("Server is broken");
        }
        [HttpGet("{PageNum}/{pageSize}")]
        public async Task<IActionResult> GetUserm([FromRoute] int PageNum, [FromRoute] int pageSize)
        {
            if (PageNum < 1)
            {
                throw new ArgumentOutOfRangeException("Pagenum must higher than 1");
            }
            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentOutOfRangeException("Page size is between 1 and 100");
            }
            var USID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (USID == null)
            {
                throw new KeyNotFoundException("Not Found User In Token");
            }
            var Manager = await context.Sysusers.FindAsync(USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            var lsUser = await context.Sysusers.Where(u => u.CuaHangId == Manager.CuaHangId)
                                    .Skip((PageNum - 1) * pageSize)
                                    .Take(pageSize).ToListAsync();
            PageRespone<Sysuser> respone = new PageRespone<Sysuser>();
            foreach (var U in lsUser)
            {
                Item<Sysuser> item = new Item<Sysuser>()
                {
                    Value = U,
                    PathChiTiet = $"admin/User/{U.UserId}"
                };
                respone.Items.Append(item);
            }

            int total = await context.Sysusers
                            .Where(u => u.CuaHangId == Manager.CuaHangId).CountAsync();
            respone.TotalCount = total;
            respone.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            respone.PageIndex = PageNum;
            respone.PageSize = pageSize;

            return Ok(respone);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetail([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Missing Param id");
            }
            Sysuser? respone = await context.Sysusers.FindAsync(id);
            if (respone == null)
            {
                throw new KeyNotFoundException("User not Exists");
            }
            return Ok(respone);
        }
    }
}