using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.EmailService;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.EmailService;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace HRMS_Infrastructure.Repository.EmailService
{
    public class EmailReportRepository : IEmailReportRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmailReportRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<DailyAbsentReportResult>> GetDailyAbsentReport()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("sp_PrepareAbsentEmployeeEmailData", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 300 // 5 minutes
                };

                // Add OUTPUT parameter
                var jsonResultParam = new SqlParameter("@JsonResult", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(jsonResultParam);

                await command.ExecuteNonQueryAsync();
                string jsonResult = command.Parameters["@JsonResult"].Value as string;

                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return new List<DailyAbsentReportResult>();
                }

                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    };

                    var result = JsonSerializer.Deserialize<List<DailyAbsentReportResult>>(jsonResult, options);
                    return result;
                }
                catch (JsonException jsonEx)
                {
                    return new List<DailyAbsentReportResult>();
                }
            }
            catch (SqlException sqlEx)
            {
                return new List<DailyAbsentReportResult>();
            }
            catch (Exception ex)
            {
                return new List<DailyAbsentReportResult>();
            }
        }

        public async Task<EmailReport?> GetEmailSendTime(string reportName)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Set a longer command timeout if needed (e.g., 120 seconds)
                    var result = await connection.QueryFirstOrDefaultAsync<EmailReport>(
                        "GetEmailSendTime",
                        new { ReportName = reportName },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120 // Optional: Increase timeout if needed
                    );

                    return result;
                }
            }
            catch (TaskCanceledException ex)
            {
                // Log the exception
                Console.WriteLine($"Task was canceled: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
