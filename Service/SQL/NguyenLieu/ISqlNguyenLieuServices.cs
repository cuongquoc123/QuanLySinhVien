using QuanLySinhVien.Models;

namespace QuanLySinhVien.Service.SQL.NguyenLieu
{
    public interface ISqlNguyenLieuServices
    {
        Task<Good?> taoNguyenLieu(string tenNL, string DVT);

        
    }
}