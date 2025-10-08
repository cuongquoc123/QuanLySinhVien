using Microsoft.AspNetCore.Mvc;
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
        Dictionary<string, string> refreshStore = new Dictionary<string, string>();
        //Thay thế hàm này bằng hàm kiểm tra người dùng thật tế có trả về UserId và Roles để Làm JWT
        private bool ValidateUser(string username, string password, out string UserId, out string[] Roles)
        {

            var user = context.Sysusers.FirstOrDefault(x => x.UserName == username);
            if (user is not null)
            {
                if (string.IsNullOrEmpty(user.Passwords))
                {
                    throw new CustomError(422, "Unprocessable Entity", "Your KeyWord not true");
                }
                if (passWordService.VerifyPassword(password, user.Passwords))
                {
                    if (user.Role != null)
                    {
                        Roles = new string[] { $"{user.Role.RoleName}" };
                    }
                    else
                    {
                        throw new KeyNotFoundException("Can't be Authentication");
                    }
                    UserId = username;
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
            ValidateUser(request.UserName, request.Password, out string userId, out string[] roles);
            var pair = _tokenService.CreateTokenPair(userId, roles);
            refreshStore[pair.RefreshToken] = userId; //Lưu refresh token vào CSDL
            return Ok(new LoginResponse(pair.AccessToken, pair.RefreshToken));
        }


        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefeshRequest request)
        {
            logger.LogInformation($"API Refesh is being called:");
            if (!refreshStore.TryGetValue(request.RefreshToken, out var userId))
            {
                return Unauthorized();
            }

            //Có cơ sở dữ liệu thật thì phải xác thực validate lại cho Token Refesh xem có hợp lệ không
            
            var user = context.Sysusers.Where(u => u.UserName == userId).First();
            if (user.Role == null)
            {
                throw new UnauthorizedAccessException("Cant Author");
            }
            if ( user.Role.RoleName is null)
            {
                throw new CustomError(403,"UnAuthentiacion","You don't have permision");
            }
            var pair = _tokenService.CreateTokenPair(userId, new string[] { user.Role.RoleName });
            refreshStore[pair.RefreshToken] = userId; //Lưu token mới vào CSDL
            //Nếu có thì mới tạo token mới
            //Nếu không thì trả về 401
            refreshStore.Remove(request.RefreshToken); //Xoá token cũ đi để tránh tấn công phát lại (replay attack)
            return Ok(new LoginResponse(pair.AccessToken, pair.RefreshToken));
        }
    }
}
