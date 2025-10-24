namespace QuanLySinhVien.DTOS.Respone
{
    public class LoginResponse
    {
        public LoginResponse(string accessToken, string username, string FullName, string Role,string avatar)
        {
            AccessToken = accessToken;
            UserName = username;
            this.FullName = FullName;
            this.Role = Role;
            this.Avatar = avatar;
        }

        public string AccessToken { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Avatar { get; set; } = null!;

    }
}