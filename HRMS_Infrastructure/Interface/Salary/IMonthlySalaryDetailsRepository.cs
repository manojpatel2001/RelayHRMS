using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface IMonthlySalaryDetailsRepository:IRepository<SalaryDetailViewModel>
    {
        Task<VMCommonResult> CreateSalaryDetails(MonthlySalaryRequestViewModel vm);
        Task<List<SalaryReportDTO>> GetMonthlySalaryData(MonthlySalaryRequestViewModel vm);
        Task<List<SalaryDetailViewModel>> GetSalaryDetails(SalaryDetailsParameterVm vm);
        Task<List<SalaryDetailViewModel>>  GetSalarySlip(salaryslipParam vm);
        Task<List<SalarySlipReport>>  GetSalarySlipReport(salaryslipParamReport vm);
        Task<List<YearlySalarySummaryVM>>  GetYearlySalarySummaryReport(int Year ,int EmpId);
        Task<List<YearlySalaryComponent>>  GetYearlySalaryCard(int Year ,int EmpId);
        Task<List<EmployeeSalaryDaysViewModel>> GetEmployeeSalaryDays(int EmpId);
        Task<List<EmployeesByBranchId>> GetEmployeesByBranchId(string BranchIds);
        Task<SalaryDetailForGetById?> GetBySalaryDetailsId(List<int> Ids);
        Task<VMCommonResult> DeleteSalaryDetails(DeleteRecordVModel deleteRecordVM);

    }
}
