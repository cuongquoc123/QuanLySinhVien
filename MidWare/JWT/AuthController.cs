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


        
        //Thay thế hàm này bằng hàm kiểm tra người dùng thật tế có trả về UserId và Roles để Làm JWT
        private bool ValidateUser(string username, string password, out Staff? User, out string Roles,out int UserId)
        {

            var user = context.Sysusers.Include(u => u.Staff).First(x => x.UserName == username);
            if (user is not null)
            {
                
                if (string.IsNullOrEmpty(user.Password))
                {
                    throw new CustomError(422, "Unprocessable Entity", "Your KeyWord not true");
                }
                if (passWordService.VerifyPassword(password, user.Password))
                {
                    var Role = context.Sysroles.Find(user.RoleId);
                    if (Role != null)
                    {
                        Roles = Role.RoleName ;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Can't be Authentication");
                    }
                    User = user.Staff;
                    UserId = user.UserId;
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
            if (!ValidateUser(request.UserName, request.Password, out var user, out string roles, out int UserId))
            {
                throw new UnauthorizedAccessException("User Infor not true");
            }
            if (user == null)
            {
                throw new UnauthorizedAccessException("User infor not true");
            }
            
            var pair = _tokenService.CreateTokenPair(UserId, roles);
            return Ok(new LoginResponse(pair.AccessToken,request.UserName,user.StaffName,roles,user.Avatar));
        }


       
    }
}
