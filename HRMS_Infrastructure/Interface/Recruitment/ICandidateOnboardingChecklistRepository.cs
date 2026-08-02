using HRMS_Core.Recruitment;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidateOnboardingChecklistRepository
    {
        Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId);
        Task<APIResponse> CreateOrUpdateChecklist(CandidateOnboardingChecklist model);
    }
}
