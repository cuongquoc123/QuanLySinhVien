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
        [HttpGet("RateLimit")]
        public IActionResult Index()
        {

            return Ok(new { Message = "Hello from APi" });
        }
        [HttpGet("")]
        public async Task<IActionResult> test_connect_db()
        {
            var data = await sheetService.StoreReview("CH001");
            
            foreach (var item in data)
            {
                System.Console.WriteLine(item[5]);
            }
            // context.TestConnect();
            return NoContent();
        }
    }
}