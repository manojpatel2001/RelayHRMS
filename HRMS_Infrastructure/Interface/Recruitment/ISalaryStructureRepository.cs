using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ISalaryStructureRepository
    {
        Task<APIResponse> GetAllTemplates(int? companyId);
        Task<APIResponse> GetTemplateById(int salaryStructureTemplateId);
        Task<APIResponse> CreateTemplate(SalaryStructureTemplate model);
        Task<APIResponse> UpdateTemplate(SalaryStructureTemplate model);
        Task<APIResponse> DeleteTemplate(DeleteRecordVM model);

        Task<APIResponse> CreateComponent(SalaryStructureComponent model);
        Task<APIResponse> UpdateComponent(SalaryStructureComponent model);
        Task<APIResponse> DeleteComponent(DeleteRecordVM model);
        Task<APIResponse> GetComponentsByTemplateId(int salaryStructureTemplateId);

        Task<APIResponse> PreviewAllowance(int salaryStructureTemplateId, decimal grossSalary, decimal? basicSalary, int companyId, bool isPFApplicable);
    }
}
