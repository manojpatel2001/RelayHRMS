using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class SalaryPayableDaysOverrideRepo: Repository<SalaryPayableDaysOverrideResponseDto>, ISalaryPayableDaysOverrideRepo
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public SalaryPayableDaysOverrideRepo(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<SP_Response> CreateSalaryPayableDaysOverride(SalaryPayableDaysOverrideResponseDto dto)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC sp_SalaryPayableDaysOverride_CRUD
                        @Operation      = {"CREATE"},
                        @EmployeeId     = {dto.EmployeeId},
                        @EmployeeCode   = {dto.EmployeeCode},
                        @MonthNumber    = {dto.MonthNumber},
                        @Year           = {dto.Year},
                        @OverridePayableDays = {dto.OverridePayableDays},
                        @AdjustmentDelta = {dto.AdjustmentDelta},
                        @Reason         = {dto.Reason},
                        @CreatedBy      = {dto.CreatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        // ──────────────────────────────────────────────
        //  UPDATE
        // ──────────────────────────────────────────────
        public async Task<SP_Response> UpdateSalaryPayableDaysOverride(SalaryPayableDaysOverrideResponseDto dto)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC sp_SalaryPayableDaysOverride_CRUD
                        @Operation      = {"UPDATE"},
                        @Id             = {dto.Id},
                        @EmployeeId     = {dto.EmployeeId},
                        @EmployeeCode   = {dto.EmployeeCode},
                        @MonthNumber    = {dto.MonthNumber},
                        @Year           = {dto.Year},
                        @OverridePayableDays = {dto.OverridePayableDays},
                        @AdjustmentDelta = {dto.AdjustmentDelta},
                        @Reason         = {dto.Reason},
                        @IsEnabled      = {dto.IsEnabled},
                        @UpdatedBy      = {dto.UpdatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        // ──────────────────────────────────────────────
        //  DELETE
        // ──────────────────────────────────────────────
        public async Task<SP_Response> DeleteSalaryPayableDaysOverride(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC sp_LoanApplication_CRUD
                    @Operation = {"DELETE"},
                    @DeletedBy = {deleteRecord.DeletedBy},
                    @LoanApplicationID = {deleteRecord.Id}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<List<SalaryPayableDaysOverridevm>> GetSalaryPayableDaysOverride(SalaryPayableDaysPara Para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@MonthNumber", Para.MonthNumber);
                    parameters.Add("@Year", Para.Year);
                    //parameters.Add("@EmployeeCodes", Para.EmployeeCodes);
                    //parameters.Add("@BranchId", Para.BranchId);
                    //parameters.Add("@AdjustmentType", Para.AdjustmentType);
                    //parameters.Add("@EmployeeCode", Model.EmployeeCodes);

                    var result = await connection.QueryAsync<SalaryPayableDaysOverridevm>(
                        "GetSalaryPayableDaysOverride",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch
            {
                return new List<SalaryPayableDaysOverridevm>();
            }
        }
    }
}
