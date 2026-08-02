using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ISelectionDecisionRepository
    {
        Task<APIResponse> CreateSelectionDecision(SelectionDecision model);
        Task<APIResponse> UpdateSelectionDecision(SelectionDecision model);
        Task<APIResponse> DeleteSelectionDecision(DeleteRecordVM model);
        Task<APIResponse> GetSelectionDecisionByCandidateApplicationId(int candidateApplicationId);
    }
}
