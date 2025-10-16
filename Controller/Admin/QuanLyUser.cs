using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("admin/User")]
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

        [HttpPut("DUser/{req}")]
        public async Task<IActionResult> DeleteUser([FromRoute]string req)
        {
            if (await sqLServices.SoftDeleteUser(req) == 200)
            {
                return Ok(new
                {
                    Message = "Delete Succesfull",
                });
            }
            throw new Exception("Server is broken");
        }
        
        
    }
}