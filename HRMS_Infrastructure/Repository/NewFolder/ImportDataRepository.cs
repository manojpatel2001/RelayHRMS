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
            return await ExecuteImportSP("sp_ImportEmployeeUpdate", jsonData , createdBy);
        }

        // Common method to execute any import SP
        private async Task<ImportSPResult> ExecuteImportSP(string spName, string jsonData, string createdBy)
        {
            var result = new ImportSPResult { Errors = new List<ImportError>() }; 

            try
            {
                var connectionString = _db.Database.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(spName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 600;

                // Add BOTH parameters
                command.Parameters.AddWithValue("@ImportData", jsonData);
                command.Parameters.AddWithValue("@CreatedBy", createdBy); // ⭐ ADD THIS

                using var reader = await command.ExecuteReaderAsync();

                // FIRST RESULT SET: Summary
                if (await reader.ReadAsync())
                {
                    result.InsertedCount = reader.GetInt32(reader.GetOrdinal("InsertedCount"));
                    result.DuplicateCount = reader.GetInt32(reader.GetOrdinal("DuplicateCount"));
                    result.BlankCount = reader.GetInt32(reader.GetOrdinal("BlankCount"));

                    // ⭐ REMOVE ErrorCount - it's not in SP output anymore
                }

                // SECOND RESULT SET: Error Details
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Errors.Add(new ImportError
                        {
                            RowNumber = reader.GetInt32(reader.GetOrdinal("RowNumber")),
                            EmployeeCode = reader.IsDBNull(reader.GetOrdinal("EmployeeCode"))
                                ? ""
                                : reader.GetString(reader.GetOrdinal("EmployeeCode")),
                            ErrorType = reader.GetString(reader.GetOrdinal("ErrorType")),
                            ErrorMessage = reader.GetString(reader.GetOrdinal("ErrorMessage"))
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
