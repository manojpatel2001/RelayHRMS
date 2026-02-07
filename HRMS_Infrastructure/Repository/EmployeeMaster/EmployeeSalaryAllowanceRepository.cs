using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class EmployeeSalaryAllowanceRepository : IEmployeeSalaryAllowanceRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeSalaryAllowanceRepository(HRMSDbContext db) 
        {
            _db = db;
        }

        public async Task<vmGetLiveEmployeeSalaryAllowance?> GetLiveEmployeeSalaryAllowance(salaryPara salaryPara)
        {
            try
            {
                var result = await _db.Set<vmGetLiveEmployeeSalaryAllowance>().FromSqlInterpolated($@"
                EXEC USP_CalculateSalaryStructure
                    @Action = {"GET"},                   
                    @GrossSalary = {salaryPara.GrossSalary},
                    @BasicSalary = {salaryPara.BasicSalary},
                    @IsPFApplicable = {salaryPara.IsPFApplicable}
                    
            ").AsNoTracking().ToListAsync();

                return result?.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }
        public async Task<VMCommonResult> CreateEmployeeSalaryAllowance(vmEmployeeSalary vmEmployeeSalary)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC USP_CalculateSalaryStructure
                    @Action = {"CREATE"},
                    @EmployeeId = {vmEmployeeSalary.EmployeeId},
                    @CompanyId = {vmEmployeeSalary.CompanyId},
                    @GrossSalary = {vmEmployeeSalary.GrossSalary},
                    @BasicSalary = {vmEmployeeSalary.BasicSalary},
                    @IsPFApplicable = {vmEmployeeSalary.IsPFApplicable}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteEmployeeSalaryAllowance(DeleteRecordVM delete)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC USP_CalculateSalaryStructure
                    @Action = {"DELETE"},
                    @Employee = {delete.Id}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<EmployeeSalaryAllowanceVM?> GetEmployeeSalaryAllowanceByEmployeeId(int EmployeeId)
        {
            try
            {
                var data= await _db.Set<EmployeeSalaryAllowanceVM>().FromSqlInterpolated($"EXEC GetEmployeeSalaryAllowanceByEmployeeId @EmployeeId = {EmployeeId}").ToListAsync();
                return data.FirstOrDefault()??null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<VMCommonResult> UpdateEmployeeSalaryAllowance(vmEmployeeSalary vmEmployeeSalary)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC USP_CalculateSalaryStructure
                    @Action = {"UPDATE"},
                    @EmployeeId = {vmEmployeeSalary.EmployeeId},
                    @CompanyId = {vmEmployeeSalary.CompanyId},
                    @GrossSalary = {vmEmployeeSalary.GrossSalary},
                     @BasicSalary = {vmEmployeeSalary.BasicSalary},
                    @IsPFApplicable = {vmEmployeeSalary.IsPFApplicable}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

       
    }
}
