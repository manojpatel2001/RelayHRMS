using HRMS_Core.ProfileManage;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ManageProfile
{
    public interface IManageProfileRepository
    {
        Task<vmPersonalInfo?> GetPersonalInfo(int employeeId);

        Task<vmContactDetails?> GetContactDetails(int employeeId);

        Task<vmSalaryDetails?> GetSalaryDetails(int employeeId);

        Task<SP_Response> UpdatePersonalInfo(vmPersonalInfo vmPersonalInfo);

        Task<SP_Response> UpdateContactDetails(vmContactDetails vmContactDetails);

        Task<SP_Response> UpdateSalaryDetails(vmSalaryDetails vmSalaryDetails);
        Task<vmGetEmployeeSalaryAllowance?> GetEmployeeSalaryAllowance(int EmployeeId);
        Task<SP_Response> UpdateProfilePic(vmUpdateEmployeeProfile model);

    }
}
