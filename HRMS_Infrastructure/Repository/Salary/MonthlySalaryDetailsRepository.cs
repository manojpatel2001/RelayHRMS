using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class MonthlySalaryDetailsRepository : Repository<MonthlySalaryDetailsRepository>, IMonthlySalaryDetailsRepository
    {

        private HRMSDbContext _db;

        public MonthlySalaryDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public Task AddAsync(SalaryDetailViewModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<VMCommonResult> CreateSalaryDetails(MonthlySalaryRequestViewModel vm)
        {

            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [USP_CalculateMonthlySalary1]
                    @StartDate = {vm.StartDate},
                    @EndDate = {vm.EndDate},
                    @EmployeeCodes = {vm.EmployeeCodes},
                    @BranchId = {vm.BranchId},
                    @Action = {vm.Action}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }


        public async Task<List<SalaryReportDTO>> GetMonthlySalaryData(MonthlySalaryRequestViewModel vm)
        {

            try
            {
                var stratdate = new SqlParameter("@StartDate", (object?)vm.StartDate ?? DBNull.Value);
                var enddate = new SqlParameter("@EndDate", (object?)vm.EndDate ?? DBNull.Value);
                var employeecodes = new SqlParameter("@EmployeeCodes", (object?)vm.EmployeeCodes ?? DBNull.Value);
                var branchidParam = new SqlParameter("@BranchId", (object?)vm.BranchId ?? DBNull.Value);
                var action = new SqlParameter("@Action", (object?)vm.Action ?? DBNull.Value);


                return await _db.Set<SalaryReportDTO>()
              .FromSqlRaw("EXEC [dbo].[USP_CalculateMonthlySalary1] @StartDate, @EndDate, @EmployeeCodes,@BranchId,@Action",
                  stratdate, enddate, employeecodes, branchidParam, action)
              .ToListAsync();
            }
            catch (Exception ex)
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
                var stratdate = new SqlParameter("@MonthNumber", (object?)vm.Month ?? DBNull.Value);
                var enddate = new SqlParameter("@Year", (object?)vm.Year ?? DBNull.Value);
                var employeecodes = new SqlParameter("@EmployeeCode", (object?)vm.EmployeeCodes ?? DBNull.Value);
                var branchidParam = new SqlParameter("@BranchId", (object?)vm.BranchId ?? DBNull.Value);


                return await _db.Set<SalaryDetailViewModel>()
              .FromSqlRaw("EXEC [dbo].[usp_GetSalaryDetailsByMonthYear] @MonthNumber, @Year, @EmployeeCode,@BranchId",
                  stratdate, enddate, employeecodes, branchidParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<SalaryDetailViewModel>();
            }
        }
    }
}
