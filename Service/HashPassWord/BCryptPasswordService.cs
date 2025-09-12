namespace QuanLySinhVien.Service.HashPassword
{
    public class BCryptPasswordService : IPassWordService
    {
        private readonly int _workFactor;
        public BCryptPasswordService(int workFactor)
        {
            _workFactor = workFactor;
        }

        public string HashPassWord(string Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password, _workFactor);
        }

        public bool VerifyPassword(string Password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(Password, hashPassword);
        }
    }
}