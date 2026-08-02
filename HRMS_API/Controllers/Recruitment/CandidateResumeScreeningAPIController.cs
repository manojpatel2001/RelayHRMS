using HRMS_API.Services;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateResumeScreeningAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResumeScreeningService _resumeScreeningService;

        public CandidateResumeScreeningAPIController(IUnitOfWork unitOfWork, ResumeScreeningService resumeScreeningService)
        {
            _unitOfWork = unitOfWork;
            _resumeScreeningService = resumeScreeningService;
        }

        [HttpGet("GetScreeningResultByApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetScreeningResultByApplicationId(int candidateApplicationId)
        {
            try
            {
                return await _unitOfWork.CandidateResumeScreeningResultRepository.GetScreeningResultByApplicationId(candidateApplicationId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve screening result. Please try again later." };
            }
        }

        [HttpGet("GetScreeningResultsByJobPositionId/{jobPositionId}")]
        public async Task<APIResponse> GetScreeningResultsByJobPositionId(int jobPositionId)
        {
            try
            {
                return await _unitOfWork.CandidateResumeScreeningResultRepository.GetScreeningResultsByJobPositionId(jobPositionId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve screening results. Please try again later." };
            }
        }

        [HttpPost("RescreenCandidateApplication/{candidateApplicationId}")]
        public async Task<APIResponse> RescreenCandidateApplication(int candidateApplicationId)
        {
            try
            {
                await _resumeScreeningService.ScreenCandidateApplicationAsync(candidateApplicationId);
                return await _unitOfWork.CandidateResumeScreeningResultRepository.GetScreeningResultByApplicationId(candidateApplicationId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to re-screen candidate application. Please try again later." };
            }
        }
    }
}
