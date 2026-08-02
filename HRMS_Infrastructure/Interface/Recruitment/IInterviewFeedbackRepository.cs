using HRMS_Core.Recruitment;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IInterviewFeedbackRepository
    {
        Task<APIResponse> SubmitFeedback(InterviewFeedback model);
        Task<APIResponse> GetFeedbackByInterviewId(int interviewId);
        Task<APIResponse> GetConsolidatedFeedbackByCandidateApplicationId(int candidateApplicationId);
    }
}
