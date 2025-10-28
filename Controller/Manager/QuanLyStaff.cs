using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.Service.SQL.StaffF;
using Microsoft.AspNetCore.Authorization;
using QuanLySinhVien.Service.ImgServices;

namespace QuanLySinhVien.Controller.Manager
{
    [ApiController]
    [Route("/manager/Staff")]
    [Authorize(Roles = "admin,manager")]
    public class QuanLyStaff : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqlStaffServices sqLServices;
        private readonly IImgService imgService;
        public QuanLyStaff(MyDbContext context, ISqlStaffServices sqLServices, IImgService imgService)
        {
            this.context = context;
            this.sqLServices = sqLServices;
            this.imgService = imgService;
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
            var ManagerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ManagerId == null)
            {
                throw new KeyNotFoundException("Missing Token");
            }
            var manager = await context.Sysusers.FindAsync(int.Parse(ManagerId));
            if (manager == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            var lsStaff = await context.Staff.Where(s => s.StoreId == manager.StoreId)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize).ToListAsync();
            if (lsStaff == null || lsStaff.Count == 0 || !lsStaff.Any())
            {
                throw new KeyNotFoundException("Can't find any Staff");
            }
            var respone = new PageRespone<StaffRespone>();

            foreach (var s in lsStaff)
            {
                if (s.DoB == null)
                {
                    throw new Exception("An Error while add DoB");
                }
                Item<StaffRespone> item = new Item<StaffRespone>()
                {

                    Value = new StaffRespone()
                    {
                        staffId = s.StaffId,
                        cccd = s.IdNumber,
                        ten = s.StaffName,
                        ngaySinh = s.DoB.Value,
                        luong = s.Salary,
                        thuong = s.Bonus,
                        avatar = s.Avatar,
                        statuSf = s.Status,
                        cuaHangId = s.StoreId
                    },
                    PathChiTiet = $"/manager/Staff/{s.StaffId}"
                };
                respone.Items.Add(item);
            }

            int total = await context.Staff.Where(s => s.StoreId == manager.StoreId).CountAsync();
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

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerId == null)
            {
                throw new KeyNotFoundException("Missing Token");
            }
            var manager = await context.Sysusers.FindAsync(int.Parse(StaffId));
            if (manager == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            if (manager.StaffId == null)
            {
                throw new UnauthorizedAccessException("user exists in store ");
            }

            Staff? respone = await context.Staff.FirstAsync(s => s.StaffId == StaffId && s.StoreId == manager.StoreId);

            if (respone == null)
            {
                throw new KeyNotFoundException("Your Store do not have this staff or you don't have permision this staff");
            }
            
            return Ok(respone);
        }

        [HttpPost]
        [Authorize(Roles = "manager,admin")]
        public async Task<IActionResult> CreateStaffm([FromForm] CreateStaffRequest staff)
        {
            if (string.IsNullOrEmpty(staff.Cccd) ||
                string.IsNullOrEmpty(staff.Ten) || string.IsNullOrEmpty(staff.DiaChi) ||
                string.IsNullOrEmpty(staff.RoleId))
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
            var user = await context.Staff.FindAsync(int.Parse(userId));
            if (user == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            if (user.RoleId == "R000000001" && !string.IsNullOrEmpty(staff.cuaHangId))
            {
                user.StoreId = staff.cuaHangId;
            }
            if (user == null)
            {
                throw new UnauthorizedAccessException("Fake Token");
            }
            try
            {
                string relativeFilePath = await imgService.SaveImgIntoProject(staff.file);
                Staff newstaff = new Staff()
                {
                    StaffName = staff.Ten,
                    IdNumber = staff.Cccd,
                    RoleId = staff.RoleId,
                    Salary = staff.Luong,
                    DoB = ngaySinh,
                    StaffAddr = staff.DiaChi,
                    StoreId = user.StoreId
                };
                var respone = await sqLServices.createStaff(newstaff, relativeFilePath);
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