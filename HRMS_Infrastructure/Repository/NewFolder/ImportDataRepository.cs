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

        public ImportDataRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<ImportSPResult> ImportAttendance(string jsonData)
        {
            return await ExecuteImportSP("sp_ImportAttendance", jsonData);
        }

        public async Task<ImportSPResult> ImportMonthlyEarnings(string jsonData)
        {
            return await ExecuteImportSP("sp_ImportMonthlyEar", jsonData);
        }

        public async Task<ImportSPResult> ImportMonthlyDeductions(string jsonData)
        {
            return await ExecuteImportSP("sp_ImportMonthlyDed", jsonData);
        }

        public async Task<ImportSPResult> ImportLeaveOpening(string jsonData)
        {
            return await ExecuteImportSP("sp_ImportLeaveOpening", jsonData);
        }

        // Common method to execute any import SP
        private async Task<ImportSPResult> ExecuteImportSP(string spName, string jsonData)
        {
            var result = new ImportSPResult();

            try
            {
                // Get connection string from DbContext
                var connectionString = _db.Database.GetConnectionString();

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(spName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 600; // 10 minutes for large data

                // Add parameter
                command.Parameters.AddWithValue("@ImportData", jsonData);

                using var reader = await command.ExecuteReaderAsync();

                // FIRST RESULT SET: Summary (InsertedCount, ErrorCount, etc.)
                if (await reader.ReadAsync())
                {
                    result.InsertedCount = reader.GetInt32(0);
                    result.ErrorCount = reader.GetInt32(1);
                    result.DuplicateCount = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    result.BlankCount = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                }

                // SECOND RESULT SET: Error Details
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Errors.Add(new ImportError
                        {
                            RowNumber = reader.GetInt32(0),
                            EmployeeCode = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            ErrorType = reader.GetString(2),
                            ErrorMessage = reader.GetString(3)
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
    
    }
}
