using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Loan;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class LoanApplicationRepository : Repository<LoanApplicationViewModel>, ILoanApplicationRepository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public LoanApplicationRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }
        public async Task<SP_Response> CreateLoanApplication(LoanApplicationViewModel model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@ApplicationDate", model.ApplicationDate);
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@ContactNo", model.ContactNo);
                parameters.Add("@EmailId", model.EmailId);
                parameters.Add("@LoanId", model.LoanId);
                parameters.Add("@LoanRequireDate", model.LoanRequireDate);
                parameters.Add("@LoanMaxLimit", model.LoanMaxLimit);
                parameters.Add("@LoanAmount", model.LoanAmount);
                parameters.Add("@InterestType", model.InterestType);
                parameters.Add("@InterestPercentage", model.InterestPercentage);
                parameters.Add("@NoOfInstallment", model.NoOfInstallment);
                parameters.Add("@InstallmentAmount", model.InstallmentAmount);
                parameters.Add("@InstallmentStartDate", model.InstallmentStartDate);
                parameters.Add("@Remark", model.Remark);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@IsDeleted", model.IsDeleted);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                    "sp_LoanApplication_CRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new SP_Response { Success = -1, ResponseMessage = "No response from the stored procedure." };
            }
        }

        public async Task<SP_Response> DeleteLoanApplication(DeleteRecordVM deleteRecord)
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

        public async Task<List<LoanMaster>> GetLoanNamesForDropdown()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<LoanMaster>(
                    "sp_GetLoanNamesForDropdown",
                    commandType: CommandType.StoredProcedure
                );
                return result.AsList();
            }
        }

        public async Task<SP_Response> UpdateLoanApplication(LoanApplicationViewModel model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@LoanApplicationID", model.LoanApplicationID);
                parameters.Add("@ApplicationDate", model.ApplicationDate);
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@ContactNo", model.ContactNo);
                parameters.Add("@EmailId", model.EmailId);
                parameters.Add("@LoanId", model.LoanId);
                parameters.Add("@LoanRequireDate", model.LoanRequireDate);
                parameters.Add("@LoanMaxLimit", model.LoanMaxLimit);
                parameters.Add("@LoanAmount", model.LoanAmount);
                parameters.Add("@InterestType", model.InterestType);
                parameters.Add("@InterestPercentage", model.InterestPercentage);
                parameters.Add("@NoOfInstallment", model.NoOfInstallment);
                parameters.Add("@InstallmentAmount", model.InstallmentAmount);
                parameters.Add("@InstallmentStartDate", model.InstallmentStartDate);
                parameters.Add("@Remark", model.Remark);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                // Execute the stored procedure
                var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                    "sp_LoanApplication_CRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new SP_Response { Success = -1, ResponseMessage = "No response from the stored procedure." };
            }
        }

        public async Task<List<GetLoanApplicationViewModel>> GetLoanApplication(int CompanyId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", CompanyId, DbType.Int32);

                    var result = (await connection.QueryAsync<GetLoanApplicationViewModel>(
                        "GetLoanApplications",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
     
                return new List<GetLoanApplicationViewModel>();
            }
        }

        public async Task<LoanApplicationViewModel> GetLoanDetailsById(int LoanId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanId", LoanId, DbType.Int32);

                    var result = await connection.QueryAsync<LoanApplicationViewModel>(
                        "GetLoanApplicationById",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
      
                return null;
            }
        }
        public async Task<List<LoanApplicationResult>> GetLoanApprovalEss(LoanApprovalSearchViewModel model)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EmpId", model.EmpId);
                parameters.Add("@CompanyId", model.CompanyId);
                parameters.Add("@SearchType", model.SearchType);
                parameters.Add("@SearchFor", model.SearchFor);
                parameters.Add("@LoanStatus", model.LoanStatus);

                var results = await db.QueryAsync<LoanApplicationResult>(
                    "[dbo].[GetLoanApprovalEss]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return results.AsList();
            }
        }
    }
}
