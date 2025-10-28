
using System.Threading.Tasks;

namespace QuanLySinhVien.Service.ImgServices
{
    public class ImgService : IImgService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImgService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> SaveImgIntoProject(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Missing File");
            }
            string wwwrootPath = webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception("Can't take web root path");
            }
            string subDirectory = "img";
            string imgPath = Path.Combine(wwwrootPath, "img");
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }

            string uniqueNameFile = Guid.NewGuid().ToString() + "_" + file.FileName;
            string FilePath = Path.Combine(imgPath, uniqueNameFile);

            using (var fileStream = new FileStream(FilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string relativeFilePath = Path.Combine("/", subDirectory, uniqueNameFile).Replace('\\', '/');

            return relativeFilePath;
        }
    }
}