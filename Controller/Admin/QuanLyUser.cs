using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody]Sysuser req)
        {
            if (ModelState.IsValid)
            {
                context.Sysusers.Add(req);
                await context.SaveChangesAsync();
                return Ok(req);
            }
            throw new Exception("Error while Add a User");
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody]Sysuser req)
        {
            if (ModelState.IsValid)
            {

                var user = await sqLServices.UpdateUser(req);
                return Ok(user);
            }
            throw new CustomError(400, "Bad Request", "Can't Update User Info");
        }

        [HttpDelete("/soft/{req}")]
        public async Task<IActionResult> SoftDeleteUser([FromRoute]string req)
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

        [HttpDelete("/{req}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string req)
        {
            if (await sqLServices.deleteUser(req) == 200)
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