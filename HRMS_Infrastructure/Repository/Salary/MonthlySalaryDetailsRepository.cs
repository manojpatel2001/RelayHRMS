using Azure.Core;
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
using static HRMS_Infrastructure.Repository.Salary.MonthlySalaryDetailsRepository;

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

        public async Task<EmployeePayableDaysResponse?> GetEmployeePayableDays(GetEmployeePayableDaysRequest request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryFirstOrDefaultAsync<EmployeePayableDaysResponse>(
                        "sp_GetEmployeePayableDaysWithMonthDays",
                        new
                        {
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            EmployeeIds = request.EmployeeIds
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return null;
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


        public async Task<List<GetSalaryvm>> GetSalaryDetails(SalaryDetailsParameterVm vm)
        {


            try
            {
                var result = await _db.Set<GetSalaryvm>().FromSqlInterpolated($"EXEC GetAllSalaryDetails @MonthNumber={vm.Month},@Year={vm.Year},@EmployeeCodes={vm.EmployeeCodes}, @BranchId ={vm.BranchId}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<GetSalaryvm>();
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


        public async Task<APIResponse> SaveFnFSettlementAsync(FnFSettlementRequest request)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Action", request.Action ?? "SaveFnF");
                parameters.Add("@Id", request.Id);

                // Employee
                parameters.Add("@EmployeeId", request.EmployeeId);
                parameters.Add("@EmployeeCode", request.EmployeeCode);
                parameters.Add("@EmployeeName", request.EmployeeName);
                parameters.Add("@CompanyId", request.CompanyId);

                // Period — NO StartDate/EndDate
                parameters.Add("@MonthNumber", request.MonthNumber);
                parameters.Add("@MonthName", request.MonthName);
                parameters.Add("@Year", request.Year);

                // Attendance
                parameters.Add("@MonthDays", request.MonthDays);
                parameters.Add("@PresentDays", request.PresentDays);
                parameters.Add("@AbsentDays", request.AbsentDays);
                parameters.Add("@Leave", request.Leave);
                parameters.Add("@WeekOff", request.WeekOff);
                parameters.Add("@Holiday", request.Holiday);
                parameters.Add("@HalfDays", request.HalfDays);
                parameters.Add("@LWPDays", request.LWPDays);
                parameters.Add("@PayableDays", request.PayableDays);
                parameters.Add("@ArrearDays", request.ArrearDays);

                // Earnings
                parameters.Add("@GrossSalary", request.GrossSalary);
                parameters.Add("@BasicSalary", request.BasicSalary);
                parameters.Add("@HRA", request.HRA);
                parameters.Add("@ConveyanceAllowance", request.ConveyanceAllowance);
                parameters.Add("@ChildEducationAllowance", request.ChildEducationAllowance);
                parameters.Add("@MedicalAllowance", request.MedicalAllowance);
                parameters.Add("@DeputationAllowance", request.DeputationAllowance);
                parameters.Add("@Arrears", request.Arrears);
                parameters.Add("@TotalGrossSalary", request.TotalGrossSalary);

                // Deductions
                parameters.Add("@PF", request.PF);
                parameters.Add("@ESIC", request.ESIC);
                parameters.Add("@ProfessionalTax", request.ProfessionalTax);
                parameters.Add("@GroupMedical", request.GroupMedical);
                parameters.Add("@TermInsurance", request.TermInsurance);
                parameters.Add("@LWF", request.LWF);
                parameters.Add("@TDS", request.TDS);
                parameters.Add("@Loan", request.Loan);
                parameters.Add("@OtherDeduction", request.OtherDeduction);
                parameters.Add("@TotalDeductions", request.TotalDeductions);
                parameters.Add("@NetSalary", request.NetSalary);

                // FnF Extras
                parameters.Add("@GratuityAmount", request.GratuityAmount);
                parameters.Add("@BonusAmount", request.BonusAmount);
                parameters.Add("@TDSAmount", request.TDSAmount);
                parameters.Add("@OtherDeductionExtra", request.OtherDeductionExtra);
                parameters.Add("@LeaveEncashDays", request.LeaveEncashDays);
                parameters.Add("@ArrearMonth", request.ArrearMonth);
                parameters.Add("@ArrearYear", request.ArrearYear);
                parameters.Add("@Remarks", request.Remarks);
                parameters.Add("@CreatedBy", request.CreatedBy);

                using var con = new SqlConnection(_connectionString);
                var result = await con.QueryFirstOrDefaultAsync<APIResponse>(
                    "sp_SaveFnFSettlement",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new APIResponse { isSuccess = false, ResponseMessage = "No response from SP." };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = "Error: " + ex.Message
                };
            }
        }

        public async Task<APIResponse> GetFullFinalStatementReport(FullFinalStatementRequestDto req)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Month", req.Month, DbType.Int32);
                    parameters.Add("@Year", req.Year, DbType.Int32);
                    parameters.Add("@EmployeeCode", req.EmployeeCode, DbType.String);

                    // ✅ dynamic use kiya — SP ke columns automatically map honge
                    var result = await connection.QueryAsync<dynamic>(
                        "SP_GetFullFinalStatementReport",
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
