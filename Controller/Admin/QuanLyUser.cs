using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("/admin/User")]
    public class AdminController : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqLServices sqLServices;

        public AdminController(MyDbContext context, ISqLServices sqLServices)
        {
            this.context = context;
            this.sqLServices = sqLServices;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest req)
        {
            if (string.IsNullOrEmpty(req.UserName) || string.IsNullOrEmpty(req.Password)
             ||string.IsNullOrEmpty(req.RoleId)  || string.IsNullOrEmpty(req.StoreId) )
            {
                throw new ArgumentException("Missing Param UserName, Password or RoleId");
            }
            Sysuser user = new Sysuser()
            {
                UserName = req.UserName,
                Passwords = req.Password,
                RoleId = req.RoleId,
                CuaHangId = req.StoreId,
            };
            try
            {
                var respone = await sqLServices.CreateUser(user);
                if (respone == null)
                {
                    throw new Exception("User can't be create");
                }
                respone.Passwords = "?????????????";
                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateUser([FromBody]Sysuser req)
        {
            if (ModelState.IsValid)
            {

                var user = await sqLServices.UpdateUser(req);
                return Ok(user);
            }
            throw new CustomError(400, "Bad Request", "Can't Update User Info");
        }

        [HttpPut("DUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (await sqLServices.SoftDeleteUser(id) == 200)
            {
                return Ok(new
                {
                    Message = "Delete Succesfull",
                });
            }
            throw new Exception("Server is broken");
        }
        [HttpGet("{PageNum}/{pageSize}")]
        public async Task<IActionResult> GetUser([FromRoute] int PageNum, [FromRoute] int pageSize)
        {
            if (PageNum < 1)
            {
                throw new ArgumentOutOfRangeException("Pagenum must higher than 1");
            }
            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentOutOfRangeException("Page size is between 1 and 100");
            }
            var lsUser = await context.Sysusers
                                    .Skip((PageNum - 1) * pageSize)
                                    .Take(pageSize).ToListAsync();
            PageRespone<Sysuser> respone = new PageRespone<Sysuser>();
            foreach (var U in lsUser)
            {

                U.Passwords = "??????????????????";
                Item<Sysuser> item = new Item<Sysuser>()
                {
                    Value = U,
                    PathChiTiet = $"admin/User/{U.UserId}"
                };
                respone.Items.Append(item);
            }

            int total = await context.Sysusers.CountAsync();
            respone.TotalCount = total;
            respone.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            respone.PageIndex = PageNum;
            respone.PageSize = pageSize;

            return Ok(respone);
        }

        [HttpGet("Detail/{id}")]
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
            respone.Passwords = "???????????????????";
            return Ok(respone);
        }
    }
}