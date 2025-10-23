using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.NguyenLieu
{
    public interface ISqlNguyenLieuServices
    {
        Task<Nguyenlieu?> taoNguyenLieu(string tenNL, string DVT);

        
    }
}