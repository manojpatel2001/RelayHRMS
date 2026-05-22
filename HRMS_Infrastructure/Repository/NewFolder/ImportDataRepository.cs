using HRMS_Core.DbContext;
using HRMS_Infrastructure.Interface.NewFolder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.NewFolder
{
    public class ImportDataRepository : IImportDataRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public ImportDataRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<ImportSPResult> ImportAttendance(string jsonData , string createdBy)
        {
            return await ExecuteImportSP("sp_ImportAttendance", jsonData, createdBy);
        }

        public async Task<ImportSPResult> ImportMonthlyEarnings(string jsonData, string createdBy)
        {
            return await ExecuteImportSP("sp_ImportMonthlyEar", jsonData, createdBy);
        }

        public async Task<ImportSPResult> ImportMonthlyDeductions(string jsonData, string createdBy)
        {
            return await ExecuteImportSP("sp_ImportMonthlyDed", jsonData, createdBy);
        }

        public async Task<ImportSPResult> ImportLeaveOpening(string jsonData, string createdBy)
        {
            return await ExecuteImportSP("sp_ImportLeaveOpening", jsonData , createdBy);
        }
        public async Task<ImportSPResult> ImportEmployeeType(string jsonData, string createdBy)
        {
            return await ExecuteImportSP("sp_ImportEmployeeType", jsonData , createdBy);
        }
        public async Task<ImportSPResult> ImportEmployeeUpdate(string jsonData, string createdBy)
        {
            return await ExecuteImportSP("sp_ImportEmployeeDesignation", jsonData , createdBy);
        }
        public async Task<ImportSPResult> SalaryPayableDays(string jsonData, string createdBy)
        {
            return await ExecuteImportSP("sp_ImportSalaryPayableDaysOverride", jsonData , createdBy);
        }

        // Common method to execute any import SP
        private async Task<ImportSPResult> ExecuteImportSP(string spName, string jsonData, string createdBy)
        {
            var result = new ImportSPResult { Errors = new List<ImportError>() };

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(spName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 600;

                command.Parameters.AddWithValue("@ImportData", jsonData);
                command.Parameters.AddWithValue("@CreatedBy", createdBy);

                using var reader = await command.ExecuteReaderAsync();

                // FIRST RESULT SET: Summary (read only available fields)
                if (await reader.ReadAsync())
                {
                    // Always read common fields
                    result.InsertedCount = GetSafeInt32(reader, "InsertedCount");
                    result.ErrorCount = GetSafeInt32(reader, "ErrorCount");
                    result.DuplicateCount = GetSafeInt32(reader, "DuplicateCount");
                    result.BlankCount = GetSafeInt32(reader, "BlankCount");

                    // Safely read optional fields (default to 0 if not present)
                    result.InvalidPayableDaysCount = GetSafeInt32(reader, "InvalidPayableDaysCount");
                }

                // SECOND RESULT SET: Error Details (if exists)
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Errors.Add(new ImportError
                        {
                            RowNumber = GetSafeInt32(reader, "RowNumber"),
                            EmployeeCode = GetSafeString(reader, "EmployeeCode"),
                            ErrorType = GetSafeString(reader, "ErrorType"),
                            ErrorMessage = GetSafeString(reader, "ErrorMessage")
                        });
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error in {spName}: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing {spName}: {ex.Message}", ex);
            }

            return result;
        }

        // Helper methods to safely read from SqlDataReader
        private int GetSafeInt32(SqlDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
            }
            catch
            {
                return 0; // Column doesn't exist or error occurred
            }
        }

        private string GetSafeString(SqlDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? "" : reader.GetString(ordinal);
            }
            catch
            {
                return ""; // Column doesn't exist or error occurred
            }
        }
    }
}
