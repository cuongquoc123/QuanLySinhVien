using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.SQL;

namespace QuanLySinhVien.Controller.Admin
{
    [ApiController]
    [Route("/admin/Product")]
    public class QuanLyProduct : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ISqLServices sqLServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        public QuanLyProduct(MyDbContext context,ISqLServices sqLServices, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.sqLServices = sqLServices;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] Sanpham sanphams, [FromForm] IFormFile file)
        {
            if (sanphams == null)
            {
                throw new ArgumentException("Missing Product ");
            }
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Missing File");
            }
            try
            {
                string wwwrootPath = webHostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(wwwrootPath))
                {
                    throw new Exception("Can't take web root path");
                }
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

                var respone = await sqLServices.CreateProDucts(sanphams, FilePath);

                if (respone == null)
                {
                    throw new Exception($"Failed to create products with img");
                }
                return Ok(new
                {
                    message = "Create Successfull",
                    data = respone
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpPut("SoftDelete/{ProductId}")]
        public async Task<IActionResult> DeleteSoft([FromRoute] string ProductId)
        {
            try
            {
                if (string.IsNullOrEmpty(ProductId))
                {
                    throw new ArgumentException("Missing Param ProductId");
                }
                int Status = await sqLServices.SoftDeleteProduct(ProductId);
                if (Status == 404)
                {
                    throw new KeyNotFoundException("Product Not Found");
                }
                if (Status == 500)
                {
                    throw new Exception("Can't soft Delete this Product");
                }
                else
                {
                    return Ok(new
                    {
                        message = "Successfully Soft Delete ProductId"
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}