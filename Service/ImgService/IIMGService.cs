namespace QuanLySinhVien.Service.ImgServices
{
    public interface IImgService
    {
        Task<string> SaveImgIntoProject(IFormFile file);
    }
}