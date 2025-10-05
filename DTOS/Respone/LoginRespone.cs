namespace QuanLySinhVien.DTOS.Respone
{
    public class LoginResponse
    {
        public LoginResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}