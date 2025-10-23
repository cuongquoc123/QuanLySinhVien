using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.DTOS.Respone;
using QuanLySinhVien.Models;
using QuanLySinhVien.Service.GGService;

namespace QuanLySinhVien.Controller
{
    [ApiController]
    [Route("test")]
    public class HomeController : ControllerBase
    {
        private readonly ISheetService sheetService;
        private readonly MyDbContext context;
        public HomeController(MyDbContext context, ISheetService sheetService)
        {
            this.context = context;
            this.sheetService = sheetService;
        }
        [HttpGet("calling")]
        public IActionResult Index()
        {

            return Ok(new { Message = "Hello from APi" });
        }
        
    }
}