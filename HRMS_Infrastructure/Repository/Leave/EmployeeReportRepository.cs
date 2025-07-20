using HRMS_Core.DbContext;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class EmployeeReportRepository : IEmployeeReport
    {

        private HRMSDbContext _db;

        public EmployeeReportRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<EmployeeAttendanceReportVm>> GetAttendanceReport(AttendanceReportVm vm)
        {
            List<EmployeeAttendanceReportVm> result = new List<EmployeeAttendanceReportVm>();

            using (SqlConnection conn = new SqlConnection(_db.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyAttendanceSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", vm.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", vm.EndDate);
                    cmd.Parameters.AddWithValue("@EmployeeCodes",
                    (vm.EmployeeCodes != null && vm.EmployeeCodes.Any())
                     ? string.Join(",", vm.EmployeeCodes)
                     : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BranchId", vm.BranchId ?? (object)DBNull.Value);

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
                                P = reader["P"] as int?,
                                A = reader["A"] as int?,
                                W = reader["W"] as int?,
                                L = reader["L"] as int?,
                                H = reader["H"] as int?
                            };

                            foreach (var col in columnNames)
                            {
                                if (Regex.IsMatch(col, @"^\d{2}$")) // D01 to D31
                                {
                                    row.Days[col] = reader[col]?.ToString();
                                }
                            }

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }


        public async Task<List<EmployeeAttendanceReportVm>> GetAttendanceReportForAdmin(AttendanceReportforAdminVm vm)
        {
            var result = new List<EmployeeAttendanceReportVm>();

            using (SqlConnection conn = new SqlConnection(_db.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyAttendanceSummaryForAdmin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", vm.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", vm.EndDate);
                    cmd.Parameters.AddWithValue("@EmployeeCodes", vm.EmployeeCodes ?? (object)DBNull.Value);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var schemaTable = reader.GetSchemaTable();
                        var columnNames = schemaTable.Rows.Cast<DataRow>()
                                                .Select(row => row["ColumnName"].ToString())
                                                .ToList();

                        while (await reader.ReadAsync())
                        {
                            // Skip header row (FullName = 'Day')
                            if (reader["FullName"]?.ToString() == "Day") continue;

                            var row = new EmployeeAttendanceReportVm
                            {
                                BranchName = reader["BranchName"]?.ToString(),
                                EmployeeCode = reader["EmployeeCode"]?.ToString(),
                                FullName = reader["FullName"]?.ToString(),
                                //Total_P = reader["Total_P"] as int?,
                                //Total_A = reader["Total_A"] as int?,
                                //Total_W = reader["Total_W"] as int?,
                                //Total_HF = reader["Total_HF"] as int?,
                            };

                            foreach (var col in columnNames)
                            {
                                if (Regex.IsMatch(col, @"^\d{2}$")) // Match "01" to "31"
                                {
                                    row.Days[col] = reader[col]?.ToString();
                                }
                            }

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }



    }
}
