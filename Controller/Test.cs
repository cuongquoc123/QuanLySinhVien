using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controller
{
    [ApiController]
    [Route("test")]
    public class HomeController : ControllerBase
    {
        private readonly MyDbContext context;
        public HomeController(MyDbContext context)
        {
            this.context = context;
        }
        [HttpGet("RateLimit")]
        public IActionResult Index()
        {

            return Ok(new { Message = "Hello from APi" });
        }
        [HttpGet("testConnectDB")]
        public IActionResult test_connect_db()
        {
            context.TestConnect();
            return NoContent();
        }
    }
}