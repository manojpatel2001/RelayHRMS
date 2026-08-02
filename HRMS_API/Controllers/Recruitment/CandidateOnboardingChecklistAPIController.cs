using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateOnboardingChecklistAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CandidateOnboardingChecklistAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetByCandidateApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.CandidateOnboardingChecklistRepository.GetByCandidateApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve checklist. Please try again later." }; }
        }

        [HttpPost("SaveChecklist")]
        public async Task<APIResponse> SaveChecklist([FromBody] CandidateOnboardingChecklist model)
        {
            try
            {
                if (model == null || model.CandidateApplicationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application is required." };

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.CandidateOnboardingChecklistRepository.CreateOrUpdateChecklist(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save checklist. Please try again later." }; }
        }
    }
}
