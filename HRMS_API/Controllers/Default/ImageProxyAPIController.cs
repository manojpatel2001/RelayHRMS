using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HRMS_API.Controllers.Default
{
    // Fetches an employee photo server-side (regardless of which host stores
    // it — mobile uploads at 163.227.92.86:8085, or any of the legacy
    // http-only employeeprofile hosts) and re-serves it from THIS API's own
    // HTTPS origin. The ESS panel runs on https://localhost:7165 — browsers
    // block loading a plain http:// image inside an https:// page ("Mixed
    // Content"), which is why employee photos never rendered here even
    // though the underlying EmployeeProfileUrl data was always correct.
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProxyAPIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private const long MaxBytes = 10 * 1024 * 1024; // 10MB safety cap

        public ImageProxyAPIController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        // GET /api/ImageProxyAPI/GetImage?url=http://...
        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url) ||
                !Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return BadRequest(new { message = "A valid absolute http(s) url is required." });
            }

            var cacheKey = $"imgproxy:{url}";
            if (_cache.TryGetValue(cacheKey, out (byte[] Bytes, string ContentType) cached))
            {
                return File(cached.Bytes, cached.ContentType);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(15);

                using var resp = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (!resp.IsSuccessStatusCode)
                    return NotFound(new { message = $"Source image returned {(int)resp.StatusCode}." });

                var contentType = resp.Content.Headers.ContentType?.MediaType ?? "";
                if (!contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { message = "Source did not return an image." });

                var bytes = await resp.Content.ReadAsByteArrayAsync();
                if (bytes.Length == 0 || bytes.Length > MaxBytes)
                    return BadRequest(new { message = "Image missing or too large." });

                _cache.Set(cacheKey, (bytes, contentType), CacheDuration);
                return File(bytes, contentType);
            }
            catch (Exception)
            {
                // Source host unreachable/slow/etc — let the caller fall back
                // to a default avatar rather than surfacing a 500.
                return NotFound(new { message = "Unable to fetch source image." });
            }
        }
    }
}
