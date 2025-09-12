namespace QuanLySinhVien.Service.HashPassword
{
    public interface IPassWordService
    {
        string HashPassWord(string Password);
        bool VerifyPassword(string Password, string hashPassword);
    }
}