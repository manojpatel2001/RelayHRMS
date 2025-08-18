using HRMS_API.NotificationService.ManageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.NotificationService.HubService
{
    [Authorize]
    public class NotificationRemainderHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.GetHttpContext()?.Request.Query["userId"];                // Or get from query string/context
                if (!string.IsNullOrEmpty(userId))
                {
                    NotificationRemainderConnectionManager.AddConnection(userId, Context.ConnectionId);
                    var connection = NotificationRemainderConnectionManager.GetConnection(userId);

                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in OnConnectedAsync: {ex.Message}");
                
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var userId = Context.GetHttpContext()?.Request.Query["userId"];  // Or get from query string/context
                if (!string.IsNullOrEmpty(userId))
                {
                    NotificationRemainderConnectionManager.RemoveConnection(userId);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in OnDisconnectedAsync: {ex.Message}");
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
