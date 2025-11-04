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
    [Authorize(Roles = "Admin,Manager")]
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
            var lsStaff = await context.Staff.Include(s => s.Role).Where(s => s.StoreId == manager.StoreId)
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
                        diaChi = s.StaffAddr,
                        avatar = s.Avatar,
                        statuSf = s.Status,
                        Email = s.Email,
                        PhoneNum = s.PhoneNum,
                        Gendar = s.Gender,
                        RoleId = s.RoleId,
                        RoleName = s.Role.RoleName
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
        [HttpPost("Create")]
        public async Task<IActionResult> CreateStaffm([FromForm] CreateStaffRequest staff)
        {
            if (string.IsNullOrEmpty(staff.cccd) ||
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
            var user = await context.Sysusers.FindAsync(int.Parse(userId));
            if (user == null)
            {
                throw new UnauthorizedAccessException("Token Not Valid");
            }
            try
            {
                System.Console.WriteLine(user.StoreId);
                string relativeFilePath = await imgService.SaveImgIntoProject(staff.file);
                Staff newstaff = new Staff()
                {
                    StaffName = staff.Ten,
                    RoleId = staff.RoleId,
                    Salary = staff.Luong,
                    IdNumber = staff.cccd,
                    DoB = ngaySinh,
                    StaffAddr = staff.DiaChi,
                    StoreId = user.StoreId,
                    Gender = staff.Gendar,
                    PhoneNum = staff.PhoneNum,
                    Email = staff.Email
                };
                System.Console.WriteLine("bắt đầu lưu");
                var staff1 = await sqLServices.createStaff(newstaff, relativeFilePath);
                System.Console.WriteLine("Lưu thành công");
                if (staff1 == null)
                {
                    throw new Exception("Can't create Staff in Database");
                }
                var respone = new StaffRespone()
                {
                    ten = staff1.StaffName,
                    staffId = staff1.StaffId,
                    cccd = staff1.Status,
                    diaChi = staff1.StaffAddr,
                    ngaySinh = staff1.DoB,
                    Gendar = staff1.Gender,
                    Email = staff1.Email,
                    avatar = staff1.Avatar,
                    statuSf = staff1.Status,
                    PhoneNum = staff1.PhoneNum
                };

                return Ok(respone);
            }
            catch (System.Exception)
            {
                throw;
            }


        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffRequest req)
        {
            try
            {
                int result = await sqLServices.UpdateStaffInfo(req);
                if (result == 404)
                {
                    throw new KeyNotFoundException("Staff not found");
                }
                return Ok(new
                {
                    message = "Update Succesfull"
                });
            }
            catch (System.Exception)
            {
                throw;
            }


        }
        [HttpPut("LayOff/{staffId}")]
        public async Task<IActionResult> LayOffStaff([FromRoute] string staffId)
        {
            try
            {
                int result = await sqLServices.SoftDeleteUser(staffId);
                if (result == 404)
                {
                    throw new KeyNotFoundException("Staff not exists");
                }
                return Ok(new
                {
                    message = "Lay Off Succesfully"
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
       
    }
}