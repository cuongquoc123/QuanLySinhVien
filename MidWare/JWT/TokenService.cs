using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;

namespace QuanLySinhVien.MidWare.JWT
{
    public class TokenPair
    {
        public string AccessToken { get; set; } = null!;

    }

    public class TokenService
    {

        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int TokenAccessMinutes; 

        public TokenService(IConfiguration config)
        {
            _key = Env.GetString("JWT_secret");
            _issuer = config["Jwt:Issuer"]!;
            _audience = config["Jwt:Audience"]!;
            TokenAccessMinutes = int.Parse(config["Jwt:AccessTokenMinutes"] ?? "15");
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public TokenPair CreateTokenPair(int userID, string roles)
        {
            // Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Thêm roles (mỗi role là 1 claim)

            claims.Add(new Claim(ClaimTypes.Role, roles));


            using var sha = SHA256.Create();
            var keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(_key));
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Access Token
            var accessToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(TokenAccessMinutes),
                signingCredentials: creds);

            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            // Refresh Token
            string refreshToken = GenerateRefreshToken();
            
            return new TokenPair
            {
                AccessToken = accessTokenString,
            };
        }
    }
}
