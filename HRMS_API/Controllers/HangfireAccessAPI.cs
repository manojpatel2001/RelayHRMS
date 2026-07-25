using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HRMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireAccessAPI : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public HangfireAccessAPI(IMemoryCache cache)
        {
            _cache = cache;
        }


        [HttpGet("get-dashboard-access")]
        public IActionResult GetDashboardAccess()
        {
            var token = Guid.NewGuid().ToString();

            // ✅ FIXED (NO MemoryCache.Default)
            _cache.Set(token, true, TimeSpan.FromMinutes(10));

            var dashboardUrl = $"/hangfire?access_token={token}";

            return Ok(new
            {
                url = dashboardUrl
            });
        }
    }
}
