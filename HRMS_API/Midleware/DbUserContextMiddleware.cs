using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace HRMS_API.Midleware
{

    public class DbUserContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public DbUserContextMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            SqlConnection? conn = null;

            try
            {
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    var userId =
                        context.User.FindFirst("UserId")?.Value
                        ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? context.User.Identity?.Name;
                   
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var cs = _config.GetConnectionString("HRMSConnection");
                        conn = new SqlConnection(cs);
                        await conn.OpenAsync();

                        using var cmd = new SqlCommand(
                            "EXEC sys.sp_set_session_context @key=N'UserId', @value=@UserId",
                            conn);

                        cmd.Parameters.AddWithValue("@UserId", userId);
                        await cmd.ExecuteNonQueryAsync();

                        context.Items["DB_CONN"] = conn;
                    }
                }

                await _next(context);
            }
            finally
            {
                conn?.Dispose(); // 🔥 must
            }
        }
    }

}
