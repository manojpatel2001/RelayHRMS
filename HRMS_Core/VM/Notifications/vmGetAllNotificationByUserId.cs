using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Notifications
{
    public class vmGetAllNotificationByUserId
    {
        public string? NotificationType { get; set; }
        public int NotificationTypeCount { get; set; }
        public int NotificationCount { get; set; }
    }
}
