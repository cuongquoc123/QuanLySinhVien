using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.Service.SQL.StaffF;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("/User")]
    public class AdminController : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqlStaffServices sqLServices;

        public AdminController(MyDbContext context, ISqlStaffServices sqLServices)
        {
            this.context = context;
            this.sqLServices = sqLServices;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUser([FromBody]createAccountRequest req)
        {
            if (string.IsNullOrEmpty(req.UserName) || string.IsNullOrEmpty(req.Password)
             ||string.IsNullOrEmpty(req.RoleId)  || string.IsNullOrEmpty(req.AccountId) )
            {
                throw new ArgumentException("Missing Param UserName, Password or RoleId");
            }
            
            try
            {
                var respone = await sqLServices.createAccount(req.AccountId,req.UserName,req.Password,req.RoleId);
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
            var lsUser = await context.Staff
                                    .Skip((PageNum - 1) * pageSize)
                                    .Take(pageSize).ToListAsync();
            PageRespone<Staff> respone = new PageRespone<Staff>();
            foreach (var U in lsUser)
            {   
                Item<Staff> item = new Item<Staff>()
                {
                    Value = U,
                    PathChiTiet = $"admin/User/{U.StaffId}"
                };
                respone.Items.Add(item);
            }

            int total = await context.Staff.CountAsync();
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
            Staff? respone = await context.Staff.FindAsync(id);
            if (respone == null)
            {
                throw new KeyNotFoundException("User not Exists");
            }
            return Ok(respone);
        }
    }
}