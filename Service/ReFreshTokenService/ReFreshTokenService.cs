using System.Collections.Concurrent;

namespace QuanLySinhVien.Services.ReFreshTokenService
{
    public class ReFreshTokenService : IReFreshTokenService
    {
        private readonly ConcurrentDictionary<string,string> _store = new ConcurrentDictionary<string,string>();
        public void AddRefreshToken(string refreshToken, string userId)
        {
            _store[refreshToken] = userId;
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _store.TryRemove(refreshToken, out _);
        }

        public bool ValidateRefreshToken(string refreshToken, out string? userId)
        {
            return _store.TryGetValue(refreshToken, out userId);
        }
    }
}