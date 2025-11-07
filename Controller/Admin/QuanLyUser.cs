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

        [HttpGet("StoreAccount")]
        public async Task<IActionResult> StoreAccount([FromQuery] string StoreId, [FromQuery] string RoleId)
        {
            try
            {
                
                var respone = await sqlStaffServices.GetStoreAccountsAsync(StoreId, RoleId);

                if (respone != null)
                {
                    return Ok(respone);
                }
                throw new Exception("Can't get account from database");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("StoreAccountPage")]
        public async Task<IActionResult> StoreAccountPage([FromQuery]int PageNum,[FromQuery] int PageSize)
        {
            try
            {
                if (PageSize > 100 || PageSize <= 0)
                {
                    throw new ArgumentOutOfRangeException("PageSize must shorter than 100 or higher than 0");
                }
                if (PageSize <= 0 )
                {
                    throw new ArgumentOutOfRangeException("PageSize mus higher than 0");
                }
                var respone = await sqlStaffServices.GetPageAccountAsync(PageNum, PageSize);

                if (respone != null)
                {
                    return Ok(respone);
                }
                throw new Exception("Can't get data from database");
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}