using QuanLySinhVien.Models;


namespace QuanLySinhVien.Service.SQL
{
    public interface ISqLServices
    {
        public Task<Sysuser> UpdateUser(Sysuser sysuser);
        public Task<int> deleteUser(string Id);
        
    }
}