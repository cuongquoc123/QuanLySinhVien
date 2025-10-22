using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.Service.SQL.StaffF;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/user")]
    public class QuanlyUser: ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqlStaffServices sqLServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        public QuanlyUser(MyDbContext context, ISqlStaffServices sqLServices,IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.sqLServices = sqLServices;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUserm([FromBody]CreateUserRequest req,[FromForm] IFormFile file)
        {
            if (req == null || string.IsNullOrEmpty(req.Ten) || string.IsNullOrEmpty(req.DiaChi) 
                || string.IsNullOrEmpty(req.DiaChi) || string.IsNullOrEmpty(req.NgaySinh) || string.IsNullOrEmpty(req.Vtri) 
                || string.IsNullOrEmpty(req.Cccd))
            {
                throw new ArgumentException("Request body is null");
            }
            var USID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (USID == null)
            {
                throw new KeyNotFoundException("Not Found User In Token");
            }
            var Manager = await context.Sysusers.Include(x => x.User).FirstAsync(x => x.UserId == USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            if (string.IsNullOrEmpty(Manager.User.CuaHangId))
            {
                throw new CustomError(403, "Forbiden", "User Fake");
            }
            if (DateOnly.TryParse(req.NgaySinh, out DateOnly date))
            {
                throw new ArgumentException("Date formate false");
            }
            Staff user = new Staff()
            {
                CuaHangId = Manager.User.CuaHangId,
                Ten = req.Ten,
                NgaySinh = date,
                Vtri = req.Vtri,
                DiaChi = req.DiaChi,
                Cccd = req.Cccd
            };


            try
            {
                
                string wwwrootPath = webHostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(wwwrootPath))
                {
                    throw new Exception("Can't take web root path");
                }
                string imgPath = Path.Combine(wwwrootPath, "img");
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }

                string uniqueNameFile = Guid.NewGuid().ToString() + "_" + file.FileName;
                string FilePath = Path.Combine(imgPath, uniqueNameFile);

                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var respone = await sqLServices.createStaff(user,FilePath);
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

        [HttpPut("DUser/{req}")]
        public async Task<IActionResult> DeleteUserm([FromRoute] string req)
        {
            var user = await context.Sysusers.FindAsync(req);
            if (user == null)
            {
                throw new KeyNotFoundException("User not exists in Database => can't delete");
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
            var Manager = await context.Sysusers.Include(x => x.User).FirstAsync(x => x.UserId ==USID);
            if (Manager == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            var lsUser = await context.Sysusers.Where(u => u.User.CuaHangId == Manager.User.CuaHangId)
                                    .Skip((PageNum - 1) * pageSize)
                                    .Take(pageSize).ToListAsync();
            PageRespone<Sysuser> respone = new PageRespone<Sysuser>();
            foreach (var U in lsUser)
            {
                U.Passwords = "?????????????????";
                Item<Sysuser> item = new Item<Sysuser>()
                {
                    Value = U,
                    PathChiTiet = $"manager/user/{U.UserId}"
                };
                respone.Items.Append(item);
            }

            int total = await context.Sysusers
                            .Where(u => u.User.CuaHangId == Manager.User.CuaHangId).CountAsync();
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
            Sysuser? respone = await context.Sysusers.Include(u => u.User).FirstAsync(u => u.UserId == id);
            if (respone == null)
            {
                throw new KeyNotFoundException("User not Exists");
            }
            respone.Passwords = "?????????????????????";
            return Ok(respone);
        }
    }
}