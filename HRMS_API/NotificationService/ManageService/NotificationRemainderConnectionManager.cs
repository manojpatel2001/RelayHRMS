using System.Collections.Concurrent;

namespace HRMS_API.NotificationService.ManageService
{
    public static class NotificationRemainderConnectionManager
    {
        private static readonly ConcurrentDictionary<string, string> _connections = new();

        /// <summary>
        /// Adds or updates a user's connection ID (replaces old connection if exists)
        /// </summary>
        public static void AddConnection(string? userId, string connectionId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    _connections.AddOrUpdate(userId, connectionId, (_, _) => connectionId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddConnection] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes a user's connection
        /// </summary>
        public static void RemoveConnection(string? userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    _connections.TryRemove(userId, out _);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RemoveConnection] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a user's connection ID (if exists)
        /// </summary>
        public static string? GetConnection(string? userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    _connections.TryGetValue(userId, out var connectionId);
                    return connectionId;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetConnection] Error: {ex.Message}");
                return null;
            }
        }


        public static IEnumerable<string> GetAllUsers()
        {
            try
            {
                return _connections.Keys;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetAllUsers] Error: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }

        public static bool IsUserOnline(string userId)
        {
            try
            {
                return _connections.ContainsKey(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IsUserOnline] Error: {ex.Message}");
                return false;
            }
        }
    }
}
