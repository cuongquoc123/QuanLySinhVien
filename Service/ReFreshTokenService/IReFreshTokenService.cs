namespace QuanLySinhVien.Services.ReFreshTokenService
{
    public interface IReFreshTokenService
    {
        void AddRefreshToken(string refreshToken, string userId);
        bool ValidateRefreshToken(string refreshToken, out string? userId);
        void RemoveRefreshToken(string refreshToken);
        
    }
}