using System.Collections.Concurrent;

namespace HRMS_API.NotificationService.ManageService
{
    public static class NotificationRemainderConnectionManager
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

        public static void AddConnection(string userId, string connectionId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            _connections.AddOrUpdate(
                userId,
                new HashSet<string> { connectionId },
                (_, existingConnections) =>
                {
                    existingConnections.Add(connectionId);
                    return existingConnections;
                }
            );
        }

        public static void RemoveConnection(string userId, string connectionId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            if (_connections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                    _connections.TryRemove(userId, out _);
            }
        }

        public static IEnumerable<string> GetConnections(string userId)
        {
            if (_connections.TryGetValue(userId, out var connections))
                return connections;
            return Enumerable.Empty<string>();
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
