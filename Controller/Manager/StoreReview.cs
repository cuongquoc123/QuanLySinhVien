using System.Security.Claims;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.GGService;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/review")]
    [Authorize(Roles = "admin,manager")]
    public class reviewController : ControllerBase
    {
        private readonly ISheetService sheetService;
        private readonly MyDbContext context;
        public reviewController(ISheetService sheetService, MyDbContext context)
        {
            this.sheetService = sheetService;
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> StoreReview()
        {
            var USID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (USID == null)
            {
                throw new KeyNotFoundException("Not Found User In Token");
            }
            var Manager = await context.Sysusers.Include(s => s.User).FirstAsync(s => s.UserId == USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            if (string.IsNullOrEmpty( Manager.User.CuaHangId ))
            {
                throw new CustomError(403, "Forbiden", "User Fake");
            }
            try
            {
                var data = await sheetService.StoreReview(Manager.User.CuaHangId.Trim());
                ReviewRespone respone = new ReviewRespone();
                respone.StoreId = Manager.User.CuaHangId.Trim();
                
                foreach (var item in data)
                {
                    if (item[1] == "")
                    {
                        item[1] = "Ẩn danh";
                    }
                    if (item[4] == "")
                    {
                        item[4] = "Không bình luận";
                    }
                    int rate;
                    if (!int.TryParse(item[3], out rate))
                    {
                        rate = 0;
                    }
                    Review newRV = new Review()
                    {
                        Time = item[0],
                        Custommer = item[1],
                        Rating = rate,
                        Staffs = item[2],
                        comment = item[4]
                    };
                    respone.reviews.Add(newRV);
                }
                if(respone.reviews.Count == 0 || !respone.reviews.Any() || respone.reviews == null)
                {
                    return NoContent();
                }
                return Ok(respone);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

    }
}