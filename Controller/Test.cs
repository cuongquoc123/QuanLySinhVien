using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien.Controller
{
    [ApiController]
    [Route("test")]
    public class HomeController : ControllerBase
    {
        [HttpGet("RateLimit")]
        public IActionResult Index()
        {
            return Ok(new { Message = "Hello from APi" });
        }
    }
}