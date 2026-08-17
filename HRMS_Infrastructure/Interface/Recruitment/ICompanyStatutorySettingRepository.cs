using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICompanyStatutorySettingRepository
    {
        Task<APIResponse> GetAll();
        Task<CompanyStatutorySetting?> GetByCompanyId(int companyId);
        Task<APIResponse> Create(CompanyStatutorySetting model);
        Task<APIResponse> Update(CompanyStatutorySetting model);
        Task<APIResponse> Delete(DeleteRecordVM model);
    }
}
