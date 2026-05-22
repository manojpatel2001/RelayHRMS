using Hangfire.Dashboard;
using Microsoft.Extensions.Caching.Memory;

namespace HRMS_API.Midleware
{


    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IMemoryCache _cache;

        public HangfireAuthorizationFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var token = httpContext.Request.Query["access_token"].ToString();

            if (string.IsNullOrEmpty(token))
                return false;

            return _cache.TryGetValue(token, out _);
        }
    }
}
