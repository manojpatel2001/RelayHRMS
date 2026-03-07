using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Salary;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class MonthlySalaryDetailsRepository : Repository<MonthlySalaryDetailsRepository>, IMonthlySalaryDetailsRepository
    {

        private HRMSDbContext _db;
        private readonly string _connectionString;

        public MonthlySalaryDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }
        public Task AddAsync(SalaryDetailViewModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<SP_Response> CreateSalaryDetails(MonthlySalaryRequestViewModel vm)
        {
            var connection = _db.Database.GetDbConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "[dbo].[USP_CalculateMonthlySalary_V2]";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 180;

                command.Parameters.Add(new SqlParameter("@StartDate", vm.StartDate));
                command.Parameters.Add(new SqlParameter("@EndDate", vm.EndDate));
                command.Parameters.Add(new SqlParameter("@EmployeeCodes", (object)vm.EmployeeCodes ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@BranchIds", (object)vm.BranchId ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CompanyId", vm.CompanyId));
                command.Parameters.Add(new SqlParameter("@Action", "Insert"));
                command.Parameters.Add(new SqlParameter("@CreatedBy", (object)vm.CreatedBy ?? DBNull.Value));

                using var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    return new SP_Response
                    {
                        ResponseMessage = reader["ResponseMessage"]?.ToString(),
                        Success = Convert.ToInt32(reader["Success"])
                    };
                }

                return new SP_Response { Success = 0, ResponseMessage = "No response from server." };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
        //public async Task<List<SalaryReportDTO>> GetMonthlySalaryData(MonthlySalaryRequestViewModel vm)
        //{

        //    try
        //    {
        //        var stratdate = new SqlParameter("@StartDate", (object?)vm.StartDate ?? DBNull.Value);
        //        var enddate = new SqlParameter("@EndDate", (object?)vm.EndDate ?? DBNull.Value);
        //        var employeecodes = new SqlParameter("@EmployeeCodes", (object?)vm.EmployeeCodes ?? DBNull.Value);
        //        var branchidParam = new SqlParameter("@BranchId", (object?)vm.BranchId ?? DBNull.Value);
        //        var action = new SqlParameter("@Action", (object?)vm.Action ?? DBNull.Value);


        //        return await _db.Set<SalaryReportDTO>()
        //      .FromSqlRaw("EXEC [dbo].[USP_CalculateMonthlySalary1] @StartDate, @EndDate, @EmployeeCodes,@BranchId,@Action",
        //          stratdate, enddate, employeecodes, branchidParam, action)
        //      .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        return new List<SalaryReportDTO>();
        //    }
        //}
        public async Task<List<APIResponse>> GetMonthlySalaryData(MonthlySalaryRequestViewModel vm)
        {
            var results = new List<dynamic>();
            string message = null;
            bool success = true;

            var connection = _db.Database.GetDbConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "[dbo].[USP_CalculateMonthlySalary_V2]";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 180;

                command.Parameters.Add(new SqlParameter("@StartDate", vm.StartDate));
                command.Parameters.Add(new SqlParameter("@EndDate", vm.EndDate));
                command.Parameters.Add(new SqlParameter("@EmployeeCodes", (object)vm.EmployeeCodes ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@BranchIds", (object)vm.BranchId ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CompanyId", vm.CompanyId));
                command.Parameters.Add(new SqlParameter("@Action", vm.Action ?? "GetData"));
                command.Parameters.Add(new SqlParameter("@CreatedBy", (object)vm.CreatedBy ?? DBNull.Value));

                using var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    var columns = Enumerable.Range(0, reader.FieldCount)
                                            .Select(i => reader.GetName(i))
                                            .ToList();

                    bool isSalaryData = columns.Any(c => c.Equals("EmployeeId", StringComparison.OrdinalIgnoreCase));
                    bool isMessageResult = columns.Any(c => c.Equals("ResponseMessage", StringComparison.OrdinalIgnoreCase));

                    if (isSalaryData)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new ExpandoObject() as IDictionary<string, object>;
                            foreach (var col in columns)
                            {
                                var ordinal = reader.GetOrdinal(col);
                                row[col] = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
                            }
                            results.Add(row);
                        }
                    }
                    else if (isMessageResult)
                    {
                        await reader.ReadAsync();
                        message = reader["ResponseMessage"]?.ToString();
                        success = Convert.ToInt32(reader["Success"]) == 1;
                    }
                }

                // Handle additional result sets (InfoMessage)
                while (await reader.NextResultAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        message = reader[0]?.ToString();
                    }
                }

                if (results.Count > 0)
                {
                    return new List<APIResponse>
            {
                new APIResponse
                {
                    isSuccess       = true,
                    ResponseMessage = message ?? "Data fetched successfully.",
                    Data            = results   // ✅ All salary rows inside Data
                }
            };
                }
                else
                {
                    return new List<APIResponse>
            {
                new APIResponse
                {
                    isSuccess       = success,
                    ResponseMessage = message ?? "No records found.",
                    Data            = null
                }
            };
                }
            }
            catch (Exception ex)
            {
                return new List<APIResponse>
        {
            new APIResponse
            {
                isSuccess       = false,
                ResponseMessage = $"Error: {ex.Message}",
                Data            = null
            }
        };
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
        public Task<IEnumerable<SalaryDetailViewModel>> GetAllAsync(Expression<Func<SalaryDetailViewModel, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task<SalaryDetailViewModel> GetAsync(Expression<Func<SalaryDetailViewModel, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(SalaryDetailViewModel entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<SalaryDetailViewModel>> GetSalaryDetails(SalaryDetailsParameterVm vm)
        {


            try
            {
                var result = await _db.Set<SalaryDetailViewModel>().FromSqlInterpolated($"EXEC GetAllSalaryDetails @MonthNumber={vm.Month},@Year={vm.Year},@EmployeeCodes={vm.EmployeeCodes}, @BranchId ={vm.BranchId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<SalaryDetailViewModel>();
            }

        }

        public async Task<VMCommonResult> DeleteSalaryDetails(DeleteRecordVModel deleteRecordVM)
        {
            try
            {
                string ids = string.Join(",", deleteRecordVM.Id); // Convert List<int> to "1,2,3"

                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
            EXEC DeleteSalaryDetails                    
                @Ids = {ids},
                @DeletedBy = {deleteRecordVM.DeletedBy}
        ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }


        public async Task<SalaryDetailForGetById?> GetBySalaryDetailsId(List<int> Id)
        {
            try
            {
                var result = await _db.Set<SalaryDetailForGetById>()
                                      .FromSqlInterpolated($"EXEC GetBySalaryDetailsId @Ids = {Id}")
                                      .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SalaryDetailViewModel>> GetSalarySlip(salaryslipParam vm)
        {

            try
            {
                var result = await _db.Set<SalaryDetailViewModel>().FromSqlInterpolated($"EXEC GetSalarySlip @MonthNumber={vm.Month},@Year={vm.Year},@EmployeeId={vm.EmployeeId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<SalaryDetailViewModel>();
            }

        }

        public async Task<List<SalarySlipReport>> GetSalarySlipReport(salaryslipParamReport vm)
        {
            try
            {
                string empIds = string.Join(",", vm.EmployeeId);
                var result = await _db.Set<SalarySlipReport>().FromSqlInterpolated($"EXEC GetAllSalaryReport @EmpIds={empIds},@Month={vm.Month},@Year={vm.Year}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<SalarySlipReport>();
            }
        }

        public async Task<List<YearlySalarySummaryVM>> GetYearlySalarySummaryReport(int Year, int EmployeeId)
        {
            try
            {

                var result = await _db.Set<YearlySalarySummaryVM>().FromSqlInterpolated($"EXEC SP_YearlySalarySummary @Year={Year},@EmployeeId={EmployeeId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<YearlySalarySummaryVM>();
            }

        }

        public async Task<List<YearlySalaryComponent>> GetYearlySalaryCard(int Year, int EmpId)
        {
            try
            {

                var result = await _db.Set<YearlySalaryComponent>().FromSqlInterpolated($"EXEC SP_YearlySalaryCrad @Year={Year},@EmployeeId={EmpId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<YearlySalaryComponent>();
            }
        }

        public async Task<List<EmployeeSalaryDaysViewModel>> GetEmployeeSalaryDays(int EmpId)
        {
            try
            {

                var result = await _db.Set<EmployeeSalaryDaysViewModel>().FromSqlInterpolated($"EXEC GetEmployeeSalaryDays @EmployeeId={EmpId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<EmployeeSalaryDaysViewModel>();
            }
        }

        public async Task<List<EmployeesByBranchId>> GetEmployeesByBranchId(string? BranchIds, int CompanyId)
        {
            try
            {

                var result = await _db.Set<EmployeesByBranchId>().FromSqlInterpolated($"EXEC GetEmployeesByBranchId @BranchIds={BranchIds},@CompanyId={CompanyId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<EmployeesByBranchId>();
            }
        }

        public async Task<List<EmployeeSalaryRegisterViewModel>> GetEmployeeSalaryRegister(
         SalaryRegisterVM Model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@MonthNumber", Model.Month);
                    parameters.Add("@Year", Model.Year);
                    parameters.Add("@CompId", Model.CompanyId);
                    //parameters.Add("@EmployeeCode", Model.EmployeeCodes);

                    var result = await connection.QueryAsync<EmployeeSalaryRegisterViewModel>(
                        "GetEmployeeSalaryRegister",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch
            {
                return new List<EmployeeSalaryRegisterViewModel>();
            }
        }

        public async Task<List<EmployeesByBranchId>> GetEmployeesForSalary(string BranchIds, int CompanyId, int? Month)
        {
            try
            {

                var result = await _db.Set<EmployeesByBranchId>().FromSqlInterpolated($"EXEC GetEmployeesForSalary @BranchIds={BranchIds},@CompanyId={CompanyId} ,@Month={Month}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<EmployeesByBranchId>();
            }
        }

        public async Task<List<EmployeeSalaryPublish>> GetEmployeeSalaryPublish(AttendanceLockParamVm model)
        {
            try
            {
                var result = await _db.Set<EmployeeSalaryPublish>()
                    .FromSqlInterpolated($"EXEC GetEmployeeSalaryPublish  @Month = {model.Month}, @Year = {model.Year}, @EmpIds = {model.EmployeeId} ,@StatusFilter={model.Status}")
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetEmployeeSalaryPublish Error: " + ex.Message);
                return new List<EmployeeSalaryPublish>();
            }
        }

        public async Task<SP_Response> UpdateSalaryPublishStatus(SalaryPublishFilterViewModel model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Month", model.Month, DbType.Int32);
                    parameters.Add("@Year", model.Year, DbType.Int32);
                    parameters.Add("@EmployeeIds", model.EmployeeIds, DbType.String);
                    parameters.Add("@SalaryIds", model.SalaryIds, DbType.String);
                    parameters.Add("@Status", model.Status, DbType.String);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "UpdateSalaryPublishStatus",  // ✅ Corrected SP name
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response
                    {
                        Success = 0,
                        ResponseMessage = "No data returned"
                    };
                }
            }
            catch (Exception ex)
            {


                return new SP_Response
                {
                    Success = 0,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SP_Response> IsPayslipPublished(PayslipFilterViewModel model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@EmployeeID", model.EmployeeId, DbType.Int32);
                    parameters.Add("@Month", model.Month, DbType.Int32);
                    parameters.Add("@Year", model.Year, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "IsPayslipPublished",  // ✅ Corrected SP name
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response
                    {
                        Success = 0,
                        ResponseMessage = "No data returned"
                    };
                }
            }
            catch (Exception ex)
            {


                return new SP_Response
                {
                    Success = 0,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<APIResponse> GetLeftEmployeedropDown(int CompanyId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", CompanyId);

                    var result = await connection.QueryAsync<dynamic>(
                        "GetLeftEmployeedropDown",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = null;
            }
            return response;
        }


        public async Task<APIResponse> GetLeftEmployeeDetails(int Employeeid)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Employeeid", Employeeid);

                    var result = await connection.QueryAsync<dynamic>(
                        "GetEmployeeDetails",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = null;
            }
            return response;
        }

  
    }
}
