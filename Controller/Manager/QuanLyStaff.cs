using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.DTOS.Request;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/Staff")]
    public class QuanLyStaff : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqLServices sqLServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        public QuanLyStaff(MyDbContext context, ISqLServices sqLServices, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.sqLServices = sqLServices;
            this.webHostEnvironment = webHostEnvironment;
        }



        [HttpGet("{pageNum}/{pageSize}")]
        public async Task<IActionResult> GetStaffm([FromRoute] int pageNum, [FromRoute] int pageSize)
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
        [HttpGet("{StaffId}")]
        public async Task<IActionResult> StaffDetailm([FromRoute] string StaffId)
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

        [HttpPost]
        public async Task<IActionResult> CreateStaffm ([FromBody] CreateStaffRequest staff, IFormFile file)
        {
            if (string.IsNullOrEmpty(staff.Cccd) || string.IsNullOrEmpty(staff.CuaHangId) ||
                string.IsNullOrEmpty(staff.Ten) || string.IsNullOrEmpty(staff.DiaChi) ||
                string.IsNullOrEmpty(staff.Vtri))
            {
                throw new ArgumentNullException("Missing Staff request body");
            }
            
            if(string.IsNullOrEmpty(staff.NgaySinh) || !DateOnly.TryParse(staff.NgaySinh,out  var ngaySinh))
            {
                throw new ArgumentException("Param NgaySInh Incorrect format or Null");
            }
            if (staff.Luong <= 0)
            {
                throw new ArgumentException("Param Luong must higher 0");
            }
            try
            {
                string WWWroot = webHostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(WWWroot))
                {
                    throw new Exception("Can't take web root path");
                }
                string imgPath = Path.Combine(WWWroot, "img");
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }
                string uniqueName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string FilePath = Path.Combine(imgPath, uniqueName);
                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                Staff newstaff = new Staff()
                {
                    Ten = staff.Ten,
                    Cccd = staff.Cccd,
                    Vtri = staff.Vtri,
                    Luong = staff.Luong,
                    NgaySinh = ngaySinh,
                    DiaChi = staff.DiaChi,
                    CuaHangId = staff.CuaHangId
                };
                var respone = await sqLServices.createStaff(newstaff, FilePath);
                if (respone == null)
                {
                    throw new Exception("Can't create Staff in Database");
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