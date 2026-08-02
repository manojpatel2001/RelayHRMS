using HRMS_API.Services;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateBGVAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;

        public CandidateBGVAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        [HttpGet("GetChecksByApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetChecksByApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.CandidateBGVRepository.GetChecksByApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve BGV checks. Please try again later." }; }
        }

        [HttpPost("CreateCheck")]
        public async Task<APIResponse> CreateCheck([FromBody] CandidateBGVCheck model)
        {
            try
            {
                if (model == null || model.CandidateApplicationId == 0 || string.IsNullOrWhiteSpace(model.CheckType))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application and check type are required." };
                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.CandidateBGVRepository.CreateCheck(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create BGV check. Please try again later." }; }
        }

        [HttpPost("UpdateCheck")]
        public async Task<APIResponse> UpdateCheck([FromBody] CandidateBGVCheck model)
        {
            try
            {
                if (model == null || model.CandidateBGVCheckId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid BGV check." };
                return await _unitOfWork.CandidateBGVRepository.UpdateCheck(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update BGV check. Please try again later." }; }
        }

        [HttpDelete("DeleteCheck")]
        public async Task<APIResponse> DeleteCheck(DeleteRecordVM model)
        {
            try { return await _unitOfWork.CandidateBGVRepository.DeleteCheck(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete BGV check. Please try again later." }; }
        }

        [HttpPost("UploadProof")]
        public async Task<APIResponse> UploadProof([FromForm] CandidateBGVCheck model, IFormFile? proofFile)
        {
            try
            {
                if (model == null || model.CandidateBGVCheckId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid BGV check." };

                if (proofFile != null && proofFile.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(proofFile, "uploads/candidate-bgv", model.ProofDocumentUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Proof upload failed. Please try again later." };
                    model.ProofDocumentUrl = fileUrl;
                }

                return await _unitOfWork.CandidateBGVRepository.UpdateCheck(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to upload proof. Please try again later." }; }
        }
    }
}
