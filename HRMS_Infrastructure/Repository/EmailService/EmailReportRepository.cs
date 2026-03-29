using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.EmailService;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface.EmailService;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        public async Task<List<DailyAbsentReportResult>> GetEmployeeEmailDataGrouped()
        {
            var flatList = new List<AbsentEmployeeEmailData>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_GetEmployeeEmailData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            flatList.Add(new AbsentEmployeeEmailData
                            {
                                EmployeeRecordId = reader.GetInt32(reader.GetOrdinal("EmployeeRecordId")),
                                BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                                AttendanceDate = reader.GetDateTime(reader.GetOrdinal("AttendanceDate")),
                                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                EmployeeCode = reader.GetString(reader.GetOrdinal("EmployeeCode")),
                                EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                                ReportingManagerId = reader.GetInt32(reader.GetOrdinal("ReportingManagerId")),
                                ReportingManagerName = reader.GetString(reader.GetOrdinal("ReportingManagerName")),
                                ReportingManagerEmail = reader.GetString(reader.GetOrdinal("ReportingManagerEmail")),
                                Attendance = reader.GetString(reader.GetOrdinal("Attendance")),
                                BranchName = reader.GetString(reader.GetOrdinal("BranchName")),
                                IsEmailSent = reader.GetBoolean(reader.GetOrdinal("IsEmailSent")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            });
                        }
                    }
                }
            }

            // ✅ GROUPING HERE
            var groupedResult = flatList
                .GroupBy(x => new
                {
                    x.ReportingManagerId,
                    x.ReportingManagerName,
                    x.ReportingManagerEmail,
                    x.AttendanceDate
                })
                .Select(g => new DailyAbsentReportResult
                {
                    ReportingManagerId = g.Key.ReportingManagerId,
                    ReportingManagerName = g.Key.ReportingManagerName,
                    ReportingManagerEmail = g.Key.ReportingManagerEmail,
                    AttendanceDate = g.Key.AttendanceDate,

                    Employees = g.Select(emp => new EmployeeRecord
                    {
                        EmployeeRecordId = emp.EmployeeRecordId,
                        EmployeeId = emp.EmployeeId,
                        BranchId = emp.BranchId,
                        EmployeeCode = emp.EmployeeCode,
                        EmployeeName = emp.EmployeeName,
                        BranchName = emp.BranchName,
                        Attendance = emp.Attendance
                    }).ToList()
                })
                .ToList();

            return groupedResult;
        }
        public async Task InsertAbsentEmployeeEmailData()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_InsertAbsentEmployeeEmailData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateEmailReport(EmailAllReport report)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_UpdateEmailReport", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("@ReportName", SqlDbType.VarChar, 200).Value = report.ReportName ?? (object)DBNull.Value;
                    command.Parameters.Add("@LastRunDate", SqlDbType.DateTime).Value = (object?)report.LastRunDate ?? DBNull.Value;
                    command.Parameters.Add("@IsForceSend", SqlDbType.Bit).Value = report.IsForceSend;
                    command.Parameters.Add("@LastError", SqlDbType.NVarChar).Value = (object?)report.LastError ?? DBNull.Value;
                    command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = (object?)report.IsSuccess ?? DBNull.Value;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        //public async Task<List<DailyAbsentReportResult>> GetDailyAbsentReport()
        //{
        //    try
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        await connection.OpenAsync();

        //        using var command = new SqlCommand("sp_PrepareAbsentEmployeeEmailData", connection)
        //        {
        //            CommandType = CommandType.StoredProcedure,
        //            CommandTimeout = 300 // 5 minutes
        //        };

        //        // Add OUTPUT parameter
        //        var jsonResultParam = new SqlParameter("@JsonResult", SqlDbType.NVarChar, -1)
        //        {
        //            Direction = ParameterDirection.Output
        //        };
        //        command.Parameters.Add(jsonResultParam);

        //        await command.ExecuteNonQueryAsync();
        //        string jsonResult = command.Parameters["@JsonResult"].Value as string;

        //        if (string.IsNullOrWhiteSpace(jsonResult))
        //        {
        //            return new List<DailyAbsentReportResult>();
        //        }

        //        try
        //        {
        //            var options = new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true,
        //                AllowTrailingCommas = true,
        //                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        //            };

        //            var result = JsonSerializer.Deserialize<List<DailyAbsentReportResult>>(jsonResult, options);
        //            return result;
        //        }
        //        catch (JsonException jsonEx)
        //        {
        //            return new List<DailyAbsentReportResult>();
        //        }
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        return new List<DailyAbsentReportResult>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<DailyAbsentReportResult>();
        //    }
        //}

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
        public async Task<List<EmailAllReport>> GetAllEmailSendTime()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Set a longer command timeout if needed (e.g., 120 seconds)
                    var result = await connection.QueryAsync<EmailAllReport>(
                        "GetAllEmailSendTime", 
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120 // Optional: Increase timeout if needed
                    );

                    return result.ToList();
                }
            }
            catch (TaskCanceledException ex)
            {
                // Log the exception
                Console.WriteLine($"Task was canceled: {ex.Message}");
                return new List<EmailAllReport>();
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<EmailAllReport>();
            }
        }

        public async Task<ManpowerRequisitionEmailViewModel> GetManpowerRequisitionEmail(int ManpowerRequisitionId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Set a longer command timeout if needed (e.g., 120 seconds)
                    var result = await connection.QueryFirstOrDefaultAsync<ManpowerRequisitionEmailViewModel>(
                        "GetManpowerRequisitionEmail",
                        new { ManpowerRequisitionId },
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

        public async Task<List<TodayLeftEmployeeEmailVM>> GetTodayLeftEmployeesEmailData()
        {
            var response = new List<TodayLeftEmployeeEmailVM>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var employees = await connection.QueryAsync<TodayLeftEmployeeEmailVM>(
                        "USP_GetTodayLeftEmployeesEmailData",
                        commandType: CommandType.StoredProcedure
                    );

                    response = employees.ToList();
                }
            }
            catch (Exception ex)
            {
                response= new List<TodayLeftEmployeeEmailVM>();
            }
            return response;
        }

        public async Task<AccessRequestViewModel> GetManpowerRequisitionEmailITandHR(int ManpowerRequisitionId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Set a longer command timeout if needed (e.g., 120 seconds)
                    var result = await connection.QueryFirstOrDefaultAsync<AccessRequestViewModel>(
                        "GetManpowerRequisitionEmail_IT_HR",
                        new { ManpowerRequisitionId },
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
