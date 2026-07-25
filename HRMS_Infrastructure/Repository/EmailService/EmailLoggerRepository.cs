using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM.EmailService;
using HRMS_Infrastructure.Interface.EmailService;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmailService
{
    public class EmailLoggerRepository : IEmailLoggerRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmailLoggerRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }


        public async Task<APIResponse> ManageEmailLoggerAsync(EmailLogger emailLogger, string action)
        {
            var result = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", emailLogger.Id);
                    parameters.Add("@FromEmail", emailLogger.FromEmail);
                    parameters.Add("@ToEmail", emailLogger.ToEmail);
                    parameters.Add("@BCCEmail", emailLogger.BCCEmail);
                    parameters.Add("@CCEmail", emailLogger.CCEmail);
                    parameters.Add("@Subject", emailLogger.Subject);
                    parameters.Add("@Body", emailLogger.Body);
                    parameters.Add("@Status", emailLogger.Status);
                    parameters.Add("@SentAt", emailLogger.SentAt);
                    parameters.Add("@AttachmentsUrl", emailLogger.AttachmentsUrl);
                    parameters.Add("@Comments", emailLogger.Comments);
                    parameters.Add("@Action", action);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    parameters.Add("@IsSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    var emailLogs = await connection.QueryAsync<EmailLogger>(
                        "usp_EmailLogger_Manage",
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120
                    );

                    result.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    result.isSuccess = parameters.Get<bool>("@IsSuccess");
                    result.Data = emailLogs;
                }
            }
            catch (Exception ex)
            {
                result.ResponseMessage = $"An error occurred: {ex.Message}";
                result.isSuccess = false;
            }
            return result;
        }
    }
}
