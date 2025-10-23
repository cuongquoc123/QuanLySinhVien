using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Service.SQL;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Service.SQL.StaffF;
using Microsoft.AspNetCore.Authorization;
using QuanLySinhVien.MidWare.Filter;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/Staff")]
    
    public class QuanLyStaff : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqlStaffServices sqLServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        public QuanLyStaff(MyDbContext context, ISqlStaffServices sqLServices, IWebHostEnvironment webHostEnvironment)
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
            var user = await context.Sysusers.Include(x => x.User).FirstAsync(x => x.UserId == userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            var lsStaff = await context.Staff.Where(s => s.CuaHangId == user.User.CuaHangId)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            if (lsStaff == null || lsStaff.Count == 0 || !lsStaff.Any())
            {
                throw new KeyNotFoundException("Can't find any Staff");
            }
            var respone = new PageRespone<StaffRespone>();

            foreach (var s in lsStaff)
            {
                if (s.NgaySinh == null)
                {
                    throw new Exception("An Error while add DoB");
                }
                if (s.Thuong == null)
                {
                    s.Thuong = 0;
                }
                if (s.Luong == null)
                {
                    s.Luong = 0;
                }
                Item<StaffRespone> item = new Item<StaffRespone>()
                {

                    Value = new StaffRespone()
                    {
                        staffId = s.StaffId,
                        cccd = s.Cccd,
                        ten = s.Ten,
                        ngaySinh = s.NgaySinh.Value,
                        luong = s.Luong.Value,
                        thuong = s.Thuong.Value,
                        avatar = s.Avatar,
                        statuSf = s.StatuSf,
                        cuaHangId = s.CuaHangId
                    },
                    PathChiTiet = $"/manager/Staff/{s.Cccd}"
                };
                respone.Items.Add(item);
            }

            int total = await context.Staff.Where(s => s.CuaHangId == user.User.CuaHangId).CountAsync();
            respone.TotalCount = total;
            respone.PageIndex = pageNum;
            respone.PageSize = pageSize;
            respone.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            return Ok(respone);
        }
        [HttpGet("Detail/{StaffId}")]
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
            var user = await context.Staff.FindAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            if (user.CuaHangId == null)
            {
                throw new UnauthorizedAccessException("user exists in store ");
            }
            Staff? respone = await context.Staff.FindAsync(StaffId);

            if (respone == null)
            {
                throw new KeyNotFoundException("Your Store do not have this staff or you don't have permision this staff");
            }
            if (respone.CuaHangId !=  user.CuaHangId)
            {
                throw new CustomError(401,"Forbidden","User not permission");
            }
            return Ok(respone);
        }

        [HttpPost]
        [Authorize(Roles = "manager,admin")]
        public async Task<IActionResult> CreateStaffm([FromForm] CreateStaffRequest staff)
        {
            if (string.IsNullOrEmpty(staff.Cccd) ||
                string.IsNullOrEmpty(staff.Ten) || string.IsNullOrEmpty(staff.DiaChi) ||
                string.IsNullOrEmpty(staff.Vtri))
            {
                throw new ArgumentNullException("Missing Staff request body");
            }
            string expectedFormat = "dd-MM-yyyy";
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            if (string.IsNullOrEmpty(staff.NgaySinh) || !DateOnly.TryParseExact(staff.NgaySinh, expectedFormat, culture, System.Globalization.DateTimeStyles.None, out var ngaySinh))
            {
                throw new ArgumentException("Param NgaySInh Incorrect format or Null");
            }
            if (staff.Luong <= 0)
            {
                throw new ArgumentException("Param Luong must higher 0");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new KeyNotFoundException("Missing Token");
            }
            var user = await context.Sysusers.Include(x => x.User).FirstAsync(x => x.UserId == userId);
            if (user.RoleId == "R000000001" && !string.IsNullOrEmpty(staff.cuaHangId))
            {
                user.User.CuaHangId = staff.cuaHangId;
            }
            if (user == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            try
            {
                string FilePath = "https://bla.edu.vn/wp-content/uploads/2025/09/avatar-fb.jpg";
                if (staff.file != null && staff.file.Length > 0)
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
                    string uniqueName = Guid.NewGuid().ToString() + "_" + staff.file.FileName;
                    FilePath = Path.Combine(imgPath, uniqueName);
                    using (var fileStream = new FileStream(FilePath, FileMode.Create))
                    {
                        await staff.file.CopyToAsync(fileStream);
                    }
                    string subDirectory = "img";
                    string relativeFilePath = Path.Combine("/", subDirectory, uniqueName).Replace('\\', '/');
                    FilePath = relativeFilePath;
                }


                Staff newstaff = new Staff()
                {
                    Ten = staff.Ten,
                    Cccd = staff.Cccd,
                    Vtri = staff.Vtri,
                    Luong = staff.Luong,
                    NgaySinh = ngaySinh,
                    DiaChi = staff.DiaChi,
                    CuaHangId = user.User.CuaHangId
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