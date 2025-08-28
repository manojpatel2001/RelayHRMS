using HRMS_Core.DbContext;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.Notifications;
using HRMS_Infrastructure.Interface.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Notifications
{
    public class NotificationRemainderRepository: INotificationRemainderRepository
    {
        private readonly HRMSDbContext _db;

        public NotificationRemainderRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<vmGetAllNotificationByUserId>> GetAllNotificationByUserId(int userId)
        {
            try
            {
                return await _db.Set<vmGetAllNotificationByUserId>()
                    .FromSqlInterpolated($"EXEC GetAllNotificationByUserId @UserId={userId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<vmGetAllNotificationByUserId>();
            }
        }

        public async Task<NotificationRemainders?> GetNotificationRemainderById(int notificationRemainderId)
        {
            try
            {
                var result = await _db.Set<NotificationRemainders>()
                    .FromSqlInterpolated($"EXEC GetNotificationById @NotificationRemainderId = {notificationRemainderId}")
                    .ToListAsync();
                return result.FirstOrDefault()??null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<SP_Response> CreateNotificationRemainder(NotificationRemainders notificationRemainder)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC ManageNotificationRemainder
                            @Action = {"CREATE"},
                            @SenderId = {notificationRemainder.SenderId},
                            @ReceiverIds = {notificationRemainder.ReceiverIds},
                            @NotificationMessage = {notificationRemainder.NotificationMessage},
                            @NotificationType = {notificationRemainder.NotificationType},
                            @NotificationAffectedId = {notificationRemainder.NotificationAffectedId}
                    ")
                    .ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> ReadNotificationRemainder(vmReadNotificationRemainder vmReadNotificationRemainder)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC ManageNotificationRemainder
                            @Action = {"READ"},
                            @NotificationType = {vmReadNotificationRemainder.NotificationType} ,   
                            @ReceiverIds = {vmReadNotificationRemainder.UserId}    
                    ")
                    .ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateNotificationRemainder(NotificationRemainders notificationRemainder)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC ManageNotificationRemainder
                            @Action = {"UPDATE"},
                            @SenderId = {notificationRemainder.SenderId},
                            @NotificationType = {notificationRemainder.NotificationType}
                    ")
                    .ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }
    }
}
