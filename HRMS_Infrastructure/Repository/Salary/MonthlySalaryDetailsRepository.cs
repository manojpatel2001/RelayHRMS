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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
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
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC [USP_CalculateMonthlySalary1] 
            @StartDate = {vm.StartDate:yyyy-MM-dd}, 
            @EndDate = {vm.EndDate:yyyy-MM-dd}, 
            @EmployeeCodes = {vm.EmployeeCodes}, 
            @CompanyId ={vm.CompanyId},
            @Action = {vm.Action ?? "Insert"},
              @CreatedBy = {vm.CreatedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
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

        public async Task<List<SalaryReportDTO>> GetMonthlySalaryData(MonthlySalaryRequestViewModel vm)
        {
            try
            {
                return await _db.Set<SalaryReportDTO>()
                    .FromSqlInterpolated($@"EXEC [dbo].[USP_CalculateMonthlySalary_V2]
                @StartDate ={vm.StartDate},
                @EndDate ={vm.EndDate},
                @EmployeeCodes ={vm.EmployeeCodes},
                @BranchIdS ={vm.BranchId},
                @CompanyId ={vm.CompanyId},
                @Action={vm.Action},       
                @CreatedBy = {vm.CreatedBy}")



                    .ToListAsync();
            }
            catch
            {
                return new List<SalaryReportDTO>();
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
                var result= await _db.Set<SalaryDetailViewModel>().FromSqlInterpolated($"EXEC GetAllSalaryDetails @MonthNumber={vm.Month},@Year={vm.Year},@EmployeeCodes={vm.EmployeeCodes}, @BranchId ={vm.BranchId}").ToListAsync();
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
    }
}
