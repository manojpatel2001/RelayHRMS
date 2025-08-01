using HRMS_Core.Employee;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface  IEmployeeDirectory
    {
        Task<List<EmployeeDirectoryResultVM>> GetEmployeeDirectoryAsync(EmpDirectorysearchVm vm);
    }
}
