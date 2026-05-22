using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IMyProfileRepository
    {
        Task<List<vmMyProfile>> GetEmployeeProfile(int employeeId, int companyId);

        Task<List<vmMyProfile>> GetEmployeeProfiles(int employeeId,int companyId);

    }
}
