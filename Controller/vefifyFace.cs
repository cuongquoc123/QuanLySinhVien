using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.CheckFace;
using QuanLySinhVien.Service.SQL;
namespace QuanLySinhVien.Controller;
[ApiController]
public class verifyFace : ControllerBase
{
    private readonly ISqLServices sqLServices;
    private readonly MyDbContext context;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly CheckFace checkFace = new CheckFace();

    private readonly ILogger<verifyFace> logger;

    public verifyFace(ILogger<verifyFace> logger, IWebHostEnvironment webHostEnvironment, MyDbContext context, ISqLServices sqLServices)
    {
        this.context = context;
        this.logger = logger;
        this.webHostEnvironment = webHostEnvironment;
        this.sqLServices = sqLServices;
    }

    [HttpPost("DiemDanh")]
    public async Task<IActionResult> DiemDanh([FromForm] IFormFile img, string mssv, string maLHP)
    {
        logger.LogInformation("Verify API is being called");
        //Kiểm tra ảnh gửi vào có hợp lệ không
        if (img == null || img.Length == 0)
        {
            logger.LogWarning("File IMG Is Null ");
            throw new ArgumentException("File is Null");
        }
        var SV = await context.SinhViens.FindAsync(mssv);
        var LHP = await  context.LopHocPhans.FindAsync(maLHP);
      


        if (LHP is null)
        {
            throw new KeyNotFoundException("Classroom not Exsit");
        }
        
        if (SV is null)
        {
            throw new KeyNotFoundException("Student not Exsit");
        }
        //Kiểm tra sinh viên có thuộc lớp học phần không 
        if (!SV.MaLopHps.Contains(LHP))
        {
            throw new ArgumentException("Do not register for class");
        }
        string IMG2 = SV.Avatar ?? string.Empty;
        var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "temp_uploads");
        if (Directory.Exists(uploadFolder))
        {  //Tạo folder chứa ảnh tạm thời nếu chưa xuất hiện
            Directory.CreateDirectory(uploadFolder);
            logger.LogInformation($"Create new folder named: {uploadFolder} ");
        }

        string IMG1 = string.Empty;
        

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
            await sqLServices.DiemDanhThanhCong(mssv, maLHP);
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