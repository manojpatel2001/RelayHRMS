using HRMS_Core.Recruitment;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IEmployeeSalaryStructureTemplateRepository
    {
        Task<APIResponse> AssignTemplateToEmployee(EmployeeSalaryStructureTemplate model);
        Task<APIResponse> UnassignEmployeeTemplate(int employeeId, int? updatedBy);
        Task<APIResponse> GetEmployeeTemplate(int employeeId);
        Task<EffectiveSalaryStructureTemplateResult?> GetEffectiveTemplate(int employeeId, int companyId, int? gradeId, int? designationId);
    }
}
