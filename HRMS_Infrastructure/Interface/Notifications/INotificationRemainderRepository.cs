using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Notifications
{
    public interface INotificationRemainderRepository
    {
        Task<List<vmGetAllNotificationByUserId>> GetAllNotificationByUserId(int userId);
        Task<NotificationRemainders?> GetNotificationRemainderById(int notificationRemainderId);
        Task<SP_Response> CreateNotificationRemainder(NotificationRemainders notificationRemainder);
        Task<SP_Response> ReadNotificationRemainder(string notificationType);
    }
}
