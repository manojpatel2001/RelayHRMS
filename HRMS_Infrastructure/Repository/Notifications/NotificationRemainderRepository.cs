using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.Notifications;
using HRMS_Infrastructure.Interface.Notifications;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Notifications
{
    public class NotificationRemainderRepository: INotificationRemainderRepository
    {
        private readonly HRMSDbContext _db;

        private readonly string _connectionString;

        public NotificationRemainderRepository(HRMSDbContext db) 
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
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

        public async Task<APIResponse> GetRemainingCompOffLeave(int EmployeeId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryAsync<dynamic>(
                        "USP_GetCompOffTransactionBalance",
                        new { @EmpId = EmployeeId},
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null||!result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "record not found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result; 
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

    }
}
