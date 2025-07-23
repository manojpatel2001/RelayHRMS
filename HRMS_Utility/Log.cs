using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Utility
{
    public static class Log
    {
        public static void LogToFile(string message)
        {
            try
            {
                var logDir = Path.Combine(AppContext.BaseDirectory, "AppLogs");
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                var logFile = Path.Combine(logDir, $"Log_{DateTime.UtcNow:yyyyMMdd}.txt");
                var logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}";
                File.AppendAllText(logFile, logMessage);
            }
            catch
            {
                // Skip logging if logging itself fails.
            }
        }
    }

}
