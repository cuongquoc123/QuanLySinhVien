namespace QuanLySinhVien.MidWare.JWT
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
        public int AccessTokenMinutes { get; set; }
        public int RefreshTokenDays { get; set; }
    }
}