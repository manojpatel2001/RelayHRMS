using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Default
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultImageAPIController : ControllerBase
    {
        [HttpGet("GetBase64Image/{fileName}")]
        public IActionResult GetBase64Image(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "default-image", fileName);
            if (!System.IO.File.Exists(path))
                return NotFound(new { message = "Image not found" });

            var bytes = System.IO.File.ReadAllBytes(path);
            var base64String = Convert.ToBase64String(bytes);

            return Ok(new { base64String });
        }
    }
}
