using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;

namespace QuanLySinhVien.MidWare.JWT
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly TokenService _tokenService;

        private readonly MyDbContext context;

        private readonly IPassWordService passWordService;
        public AuthController(TokenService tokenService, ILogger<AuthController> logger, MyDbContext context, IPassWordService passWordService)
        {
            _tokenService = tokenService;
            this.logger = logger;
            this.context = context;
            this.passWordService = passWordService;
        }

        //Chưa có cơ sở dữ liệu nên phải làm tạm thế này, có rồi phải luư token vào CSDL và lấy ra để làm Refesh
        
        //Thay thế hàm này bằng hàm kiểm tra người dùng thật tế có trả về UserId và Roles để Làm JWT
        private bool ValidateUser(string username, string password, out Staff? User, out string Roles)
        {

            var user = context.Sysusers.Include(s => s.User).First(x => x.UserName == username);
            if (user is not null)
            {
                if (string.IsNullOrEmpty(user.User.StatuSf))
                {
                    throw new KeyNotFoundException("Server can't find user Status");
                }
                if (user.User.StatuSf.Equals("Nghỉ Việc") )
                {
                    User = null;
                    Roles =  string.Empty ;
                    return false;
                }
                if (string.IsNullOrEmpty(user.Passwords))
                {
                    throw new CustomError(422, "Unprocessable Entity", "Your KeyWord not true");
                }
                if (passWordService.VerifyPassword(password, user.Passwords))
                {
                    var Role = context.Sysroles.Find(user.User.RoleId);
                    if (Role != null)
                    {
                        Roles = Role.RoleName ;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Can't be Authentication");
                    }
                    User = user.User;
                    return true;
                }
                else
                {
                    throw new KeyNotFoundException("Can't be Authentication");
                }
            }
            else
            {
                throw new KeyNotFoundException("User Not Exsits");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            logger.LogInformation($"API Login is being called:");
            if (!ValidateUser(request.UserName, request.Password, out var user, out string roles))
            {
                throw new UnauthorizedAccessException("User Infor not true");
            }
            if (user == null)
            {
                throw new UnauthorizedAccessException("User infor not true");
            }
            var SysU = context.Sysusers.Find(user.StaffId);
            if (SysU == null)
            {
                logger.LogCritical("User not found by can valid");
                throw new UnauthorizedAccessException("User not valid");
            }
            if (string.IsNullOrEmpty(SysU.UserName))
            {
                logger.LogWarning("Username is null or empty");
                throw new UnauthorizedAccessException("User not valid");
            }
            var pair = _tokenService.CreateTokenPair(user.StaffId, roles);
            return Ok(new LoginResponse(pair.AccessToken,SysU.UserName,user.Ten,roles,user.Avatar));
        }


       
    }
}
