using Hangfire.Dashboard;

namespace HRMS_API.Midleware
{
    public class HangfireNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true; // Allow all users to access Hangfire dashboard
        }
    }
}
