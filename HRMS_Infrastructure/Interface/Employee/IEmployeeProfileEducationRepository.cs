using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Repository.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeProfileEducationRepository
    {

        
        Task<SP_Response> CreateEmployeeEducation (VmEducation model);
        Task<SP_Response> UpdateEmployeeEducation (VmEducation model);
        Task<SP_Response> DeleteEmployeeEducation (VmEducation model);

        Task<List<VmEducation>> GetAllEmployeeEducation();

    }
}
