using HRMS_Core.Employee;
using HRMS_Core.Loan;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeTransferRepository : IRepository<EmployeeTransfer>
    {
        Task<SP_Response> CreateEmployeeTransfer(EmployeeTransfer model);
        Task<SP_Response> UpdateEmployeeTransfer(EmployeeTransfer model);
        Task<SP_Response> DeleteEmployeeTransfer(DeleteRecordVM deleteRecord);
        Task<List<GetEmployeeTransfervm>> GetEmployeeTransfers(int? CompanyId);
    }
}
