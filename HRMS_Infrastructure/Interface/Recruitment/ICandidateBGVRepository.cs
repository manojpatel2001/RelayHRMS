using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidateBGVRepository
    {
        Task<APIResponse> GetChecksByApplicationId(int candidateApplicationId);
        Task<APIResponse> CreateCheck(CandidateBGVCheck model);
        Task<APIResponse> UpdateCheck(CandidateBGVCheck model);
        Task<APIResponse> DeleteCheck(DeleteRecordVM model);
    }
}
