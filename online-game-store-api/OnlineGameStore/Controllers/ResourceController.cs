using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace OnlineGameStore.Controllers
{
    [Route("api")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ResourceController(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }

        [HttpGet("payment/images/{path}")]
        public IActionResult GetImage(string path)
        {
            string pathToImage = Path.Combine(_environment.ContentRootPath, "Resources", "Payment", path);
            return PhysicalFile(pathToImage, "image/jpeg");
        }
    }
}
