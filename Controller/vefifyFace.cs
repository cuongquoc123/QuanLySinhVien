using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.CheckFace;
namespace QuanLySinhVien.Controllers;
[ApiController]
public class verifyFace : ControllerBase
{
    private readonly MyDbContext context;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly CheckFace checkFace = new CheckFace();

    private readonly ILogger<verifyFace> logger;

    public verifyFace(ILogger<verifyFace> logger, IWebHostEnvironment webHostEnvironment, MyDbContext context)
    {
        this.context = context;
        this.logger = logger;
        this.webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("Verify")]
    public async Task<IActionResult> VerifyFace([FromForm] IFormFile img, string mssv)
    {
        logger.LogInformation("Verify API is being called");
        if (img == null || img.Length == 0)
        {
            logger.LogWarning("File IMG Is Null ");
            throw new ArgumentException("File is Null");
        }
        var SV = context.SinhViens.Where(i => i.Mssv == mssv).First();
        if (SV == null)
        {
            throw new KeyNotFoundException("Sinh Vien not Exsit");
        }
        string IMG2 = SV.Avatar ?? null;
        var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "temp_uploads");
        if (Directory.Exists(uploadFolder))
        {  //Tạo folder chứa ảnh tạm thời nếu chưa xuất hiện
            Directory.CreateDirectory(uploadFolder);
            logger.LogInformation($"Create new folder named: {uploadFolder} ");
        }

        string IMG1 = null;
        

        try
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
            IMG1 = Path.Combine(uploadFolder, fileName);
            using (var stream = new FileStream(IMG1, FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }
            logger.LogInformation($"Upload Temp file at: {IMG1}");

            await checkFace.CheckFaceAsync(IMG1, IMG2);
            logger.LogInformation("Verify successfull for: ");
            return Ok(new { message = "Verify successfull" });

        }
        catch (System.Exception)
        {
            logger.LogError("Error During Face Verification");
            throw;
        }
        finally
        {
            if (!string.IsNullOrEmpty(IMG1) && System.IO.File.Exists(IMG1))
            {
                System.IO.File.Delete(IMG1);
                logger.LogInformation($"Delete File Temp: {IMG1}");
            }
        }
    }


}