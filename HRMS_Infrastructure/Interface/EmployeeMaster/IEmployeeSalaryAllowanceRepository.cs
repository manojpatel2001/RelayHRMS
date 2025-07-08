using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IEmployeeSalaryAllowanceRepository
    {
        Task<VMCommonResult> CreateEmployeeSalaryAllowance(vmEmployeeSalary vmEmployeeSalary);
        Task<VMCommonResult> UpdateEmployeeSalaryAllowance(vmEmployeeSalary vmEmployeeSalary);
        Task<VMCommonResult> DeleteEmployeeSalaryAllowance(DeleteRecordVM delete);
        Task<EmployeeSalaryAllowanceVM?> GetEmployeeSalaryAllowanceByEmployeeId(int EmployeeId);
    }
}
