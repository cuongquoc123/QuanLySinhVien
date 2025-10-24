using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.DTOS.Request;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.MidWare.Filter;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.HashPassword;
using QuanLySinhVien.Services.ReFreshTokenService;

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
        private readonly IReFreshTokenService reFreshTokenService;
        public AuthController(TokenService tokenService, ILogger<AuthController> logger, MyDbContext context, IPassWordService passWordService, IReFreshTokenService reFreshTokenService)
        {
            _tokenService = tokenService;
            this.logger = logger;
            this.context = context;
            this.passWordService = passWordService;
            this.reFreshTokenService = reFreshTokenService;
        }

        //Chưa có cơ sở dữ liệu nên phải làm tạm thế này, có rồi phải luư token vào CSDL và lấy ra để làm Refesh
        
        //Thay thế hàm này bằng hàm kiểm tra người dùng thật tế có trả về UserId và Roles để Làm JWT
        private bool ValidateUser(string username, string password, out string? UserId, out string Roles)
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
                    UserId = null;
                    Roles =  string.Empty ;
                    return false;
                }
                if (string.IsNullOrEmpty(user.Passwords))
                {
                    throw new CustomError(422, "Unprocessable Entity", "Your KeyWord not true");
                }
                if (passWordService.VerifyPassword(password, user.Passwords))
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
            if (!ValidateUser(request.UserName, request.Password, out string? userId, out string roles))
            {
                throw new UnauthorizedAccessException("User Infor not true");
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User infor not true");
            }

            var pair = _tokenService.CreateTokenPair(userId, roles);
            reFreshTokenService.AddRefreshToken(pair.RefreshToken, userId);
            return Ok(new LoginResponse(pair.AccessToken, pair.RefreshToken));
        }


        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefeshRequest request)
        {
            if (!reFreshTokenService.ValidateRefreshToken(request.RefreshToken, out var userId))
            {
                throw new UnauthorizedAccessException("Refresh Token Not Valid");
            }

            //Có cơ sở dữ liệu thật thì phải xác thực validate lại cho Token Refesh xem có hợp lệ không
            
            var user = context.Sysusers.Where(u => u.UserName == userId).First();
            if (user.Role == null)
            {
                throw new UnauthorizedAccessException("You don't have permision");
            }
            if ( user.Role.RoleName is null)
            {
                throw new CustomError(403,"UnAuthentiacion","You don't have permision");
            }
            var pair = _tokenService.CreateTokenPair(user.UserId, user.Role.RoleName);
             //Lưu token mới vào CSDL
            reFreshTokenService.AddRefreshToken(pair.RefreshToken, user.UserId);
            //Nếu có thì mới tạo token mới
            //Nếu không thì trả về 401
            reFreshTokenService.RemoveRefreshToken(request.RefreshToken); //Xoá token cũ đi để tránh tấn công phát lại (replay attack)
            return Ok(new LoginResponse(pair.AccessToken, string.Empty));
        }
    }
}
