using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeProfileExperienceRepository
    {
        Task<SP_Response> CreateEmployeeExperience(VmExperience model);
        Task<SP_Response> UpdateEmployeeExperience(VmExperience model);
        Task<SP_Response> DeleteEmployeeExperience(VmExperience model);

        Task<List<VmExperience>> GetAllEmployeeExperience();
    }
}
