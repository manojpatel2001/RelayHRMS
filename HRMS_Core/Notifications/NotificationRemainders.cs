using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Notifications
{
    public class NotificationRemainders
    {
        public int NotificationRemainderId {  get; set; }
        public string? SenderId {  get; set; }
        public string? ReceiverIds {  get; set; }
        public string? NotificationMessage {  get; set; }
        public string? NotificationType {  get; set; }
        public int? NotificationAffectedId {  get; set; }
        public bool? IsRead {  get; set; }=false;
        public DateTime? NotificationTime {  get; set; }=DateTime.UtcNow;
        public bool? IsDeleted {  get; set; }=false;
    }
}
