using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewFeedbackAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewFeedbackAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("SubmitFeedback")]
        public async Task<APIResponse> SubmitFeedback([FromBody] InterviewFeedback model)
        {
            try
            {
                if (model == null || model.InterviewId == 0 || model.PanelistEmployeeId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Interview and panelist are required." };

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.InterviewFeedbackRepository.SubmitFeedback(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to submit feedback. Please try again later." };
            }
        }

        [HttpGet("GetFeedbackByInterviewId/{interviewId}")]
        public async Task<APIResponse> GetFeedbackByInterviewId(int interviewId)
        {
            try { return await _unitOfWork.InterviewFeedbackRepository.GetFeedbackByInterviewId(interviewId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve feedback. Please try again later." }; }
        }

        [HttpGet("GetConsolidatedFeedback/{candidateApplicationId}")]
        public async Task<APIResponse> GetConsolidatedFeedback(int candidateApplicationId)
        {
            try { return await _unitOfWork.InterviewFeedbackRepository.GetConsolidatedFeedbackByCandidateApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve consolidated feedback. Please try again later." }; }
        }
    }
}
