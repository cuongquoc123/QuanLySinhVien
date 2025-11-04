using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Service.SQL.StaffF;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("/admin/User")]
    public class QuanLyUser : ControllerBase
    {
        private readonly ISqlStaffServices sqlStaffServices;

        public QuanLyUser(ISqlStaffServices sqlStaffServices)
        {
            this.sqlStaffServices = sqlStaffServices;
        }

        [HttpPut]
        public async Task<IActionResult> AssaginUserToStaff([FromRoute] string StaffId, string UserName)
        {
            try
            {
                string message = await sqlStaffServices.AssignUserToStaff(StaffId, UserName);
                return Ok(new
                {
                    message = message
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}