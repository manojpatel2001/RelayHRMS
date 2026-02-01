using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class EmployeeReportRepository : IEmployeeReport
    {

        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeReportRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<EmployeeAttendanceReportVm>> GetAttendanceReport(AttendanceReportVm vm)
        {
            List<EmployeeAttendanceReportVm> result = new List<EmployeeAttendanceReportVm>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_db.Database.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyAttendanceSummary_V2", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", vm.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", vm.EndDate);
                        string employeeIdsString = vm.EmployeeIds != null && vm.EmployeeIds.Any()
                            ? string.Join(",", vm.EmployeeIds)
                            : null;
                        cmd.Parameters.AddWithValue("@EmployeeIds", (object)employeeIdsString ?? DBNull.Value);

                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var schemaTable = reader.GetSchemaTable();
                            var columnNames = schemaTable.Rows.Cast<DataRow>()
                                .Select(row => row["ColumnName"].ToString())
                                .ToList();

                            while (await reader.ReadAsync())
                            {
                                var row = new EmployeeAttendanceReportVm
                                {
                                    BranchName = reader["BranchName"]?.ToString(),
                                    EmployeeCode = reader["EmployeeCode"]?.ToString(),
                                    FullName = reader["FullName"]?.ToString(),
                                    TotalP = reader["TotalP"] != DBNull.Value ? Convert.ToDecimal(reader["TotalP"]) : 0,
                                    TotalA = reader["TotalA"] != DBNull.Value ? Convert.ToDecimal(reader["TotalA"]) : 0,
                                    TotalW = reader["TotalW"] != DBNull.Value ? Convert.ToDecimal(reader["TotalW"]) : 0,
                                    TotalL = reader["TotalL"] != DBNull.Value ? Convert.ToDecimal(reader["TotalL"]) : 0,
                                    TotalH = reader["TotalH"] != DBNull.Value ? Convert.ToDecimal(reader["TotalH"]) : 0,
                                    TotalHFLeave = reader["TotalHFLeave"] != DBNull.Value ? Convert.ToDecimal(reader["TotalHFLeave"]) : 0,
                                    TotalCO = reader["TotalCO"] != DBNull.Value ? Convert.ToDecimal(reader["TotalCO"]) : 0,
                                    TotalLWP = reader["TotalLWP"] != DBNull.Value ? Convert.ToDecimal(reader["TotalLWP"]) : 0,
                                    TotalPayableDays = reader["TotalPayableDays"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPayableDays"]) : 0,
                                    TotalUnpaidDays = reader["TotalUnpaidDays"] != DBNull.Value ? Convert.ToDecimal(reader["TotalUnpaidDays"]) : 0,
                                    TotalMonthDays = reader["TotalMonthDays"] != DBNull.Value ? Convert.ToDecimal(reader["TotalMonthDays"]) : 0,
                                    Days = new Dictionary<string, string>() // Initialize the dictionary
                                };

                                // Populate the Days dictionary for dynamic columns (e.g., D01, D02, etc.)
                                foreach (var col in columnNames)
                                {
                                    if (Regex.IsMatch(col, @"^\d{2}$"))
                                    {
                                        row.Days[col] = reader[col]?.ToString();
                                    }
                                }

                                result.Add(row);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL error (e.g., stored procedure error, connection issue, etc.)
                // You can also rethrow or return a custom error message
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception($"An error occurred while retrieving attendance data: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log any other unexpected errors
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }

            return result;
        }


        public async Task<List<EmployeeAttendanceReportVm>> GetAttendanceReportForAdmin(AttendanceReportforAdminVm vm)
        {
            throw new NotImplementedException();
            //List<EmployeeAttendanceReportVm> result = new List<EmployeeAttendanceReportVm>();

            //using (SqlConnection conn = new SqlConnection(_db.Database.GetConnectionString()))
            //{
            //    using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyAttendanceSummary", conn))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@StartDate", vm.StartDate);
            //        cmd.Parameters.AddWithValue("@EndDate", vm.EndDate);

            //        // Handle EmployeeIds parameter
            //        string employeeIdsString = vm.EmployeeIds != null && vm.EmployeeIds.Any()
            //            ? string.Join(",", vm.EmployeeIds)
            //            : null;
            //        cmd.Parameters.AddWithValue("@EmployeeIds", (object)employeeIdsString ?? DBNull.Value);

            //        // Handle BranchIds parameter (new addition)
            //        string branchIdsString = vm.BranchIds != null && vm.BranchIds.Any()
            //            ? string.Join(",", vm.BranchIds)
            //            : null;
            //        cmd.Parameters.AddWithValue("@BranchIds", (object)branchIdsString ?? DBNull.Value);

            //        await conn.OpenAsync();

            //        using (var reader = await cmd.ExecuteReaderAsync())
            //        {
            //            var schemaTable = reader.GetSchemaTable();
            //            var columnNames = schemaTable.Rows.Cast<DataRow>()
            //                                .Select(row => row["ColumnName"].ToString())
            //                                .ToList();

            //            while (await reader.ReadAsync())
            //            {
            //                var row = new EmployeeAttendanceReportVm
            //                {
            //                    BranchName = reader["BranchName"]?.ToString(),
            //                    EmployeeCode = reader["EmployeeCode"]?.ToString(),
            //                    FullName = reader["FullName"]?.ToString(),
            //                    P = reader["P"]?.ToString(),
            //                    A = reader["A"]?.ToString(),
            //                    W = reader["W"]?.ToString(),
            //                    L = reader["L"]?.ToString(),
            //                    H = reader["H"]?.ToString()
            //                };

            //                foreach (var col in columnNames)
            //                {
            //                    if (Regex.IsMatch(col, @"^\d{2}$")) // D01 to D31
            //                    {
            //                        row.Days[col] = reader[col]?.ToString();
            //                    }
            //                }

            //                result.Add(row);
            //            }
            //        }
            //    }
            //}
            //return result;
        }


        public async Task<List<ShiftReportVm>> GetShiftReport(AttendanceReportVm vm)
        {
            var result = new List<ShiftReportVm>();
            try
            {
                using (var conn = new SqlConnection(_db.Database.GetConnectionString()))
                {
                    using (var cmd = new SqlCommand("GetShiftReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", vm.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", vm.EndDate);
                        string employeeIdsString = vm.EmployeeIds != null && vm.EmployeeIds.Any()
                            ? string.Join(",", vm.EmployeeIds)
                            : null;
                        cmd.Parameters.AddWithValue("@EmployeeIds", (object)employeeIdsString ?? DBNull.Value);

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var schemaTable = reader.GetSchemaTable();
                            var columnNames = schemaTable.Rows.Cast<DataRow>()
                                .Select(row => row["ColumnName"].ToString())
                                .ToList();

                            while (await reader.ReadAsync())
                            {
                                var row = new ShiftReportVm
                                {
                                    EmployeeCode = reader["EmployeeCode"]?.ToString(),
                                    FullName = reader["FullName"]?.ToString(),
                                    SortOrder = reader["SortOrder"] != DBNull.Value ? Convert.ToInt32(reader["SortOrder"]) : null,
                                    Days = new ExpandoObject()
                                };

                                // Dynamic day columns fill
                                foreach (var col in columnNames)
                                {
                                    if (Regex.IsMatch(col, @"^\d{2}$")) // 01 se 31 tak din
                                    {
                                        ((IDictionary<string, object>)row.Days)[col] = reader[col]?.ToString();
                                    }
                                }

                                result.Add(row);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error occurred while retrieving shift report: {sqlEx.Message}", sqlEx);
            }
            catch (InvalidCastException castEx)
            {
                throw new Exception($"Data conversion error occurred while processing shift report: {castEx.Message}", castEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred while retrieving shift report: {ex.Message}", ex);
            }

            return result;
        }



public async Task<(List<vmAttedanceCalanderDays> AttedanceCalanderDays, List<vmAttedanceCalanderDaysSummary> AttedanceCalanderDaysSummary)> GetAttendanceCalender(CommonParameter commonParameter)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EmployeeId", commonParameter.EmployeeId);
                    queryParameters.Add("@Year", commonParameter.Year);
                    queryParameters.Add("@Month", commonParameter.Month);

                    using (var multi = await connection.QueryMultipleAsync(
                        "sp_EmployeeMonthlyAttendanceCalendar",
                        queryParameters,
                        commandType: CommandType.StoredProcedure))
                    {
                        try
                        {
                            var AttedanceCalanderDays = (await multi.ReadAsync<vmAttedanceCalanderDays>()).AsList();
                            var AttedanceCalanderDaysSummary = (await multi.ReadAsync<vmAttedanceCalanderDaysSummary>()).AsList();

                            return (AttedanceCalanderDays, AttedanceCalanderDaysSummary);
                        }
                        catch (Exception ex)
                        {
                            return (new List<vmAttedanceCalanderDays>(), new List<vmAttedanceCalanderDaysSummary>());
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                return (new List<vmAttedanceCalanderDays>(), new List<vmAttedanceCalanderDaysSummary>());
            }
        }

     
    }
}
