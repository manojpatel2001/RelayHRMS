using HRMS_API.Services;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;

        public CandidateAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        [HttpGet("GetAllCandidates")]
        public async Task<APIResponse> GetAllCandidates()
        {
            try
            {
                return await _unitOfWork.CandidateRepository.GetAllCandidates();
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve candidates. Please try again later." };
            }
        }

        [HttpGet("GetCandidateById/{candidateId}")]
        public async Task<APIResponse> GetCandidateById(int candidateId)
        {
            try
            {
                return await _unitOfWork.CandidateRepository.GetCandidateById(candidateId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve candidate. Please try again later." };
            }
        }

        [HttpGet("GetCandidateFullProfile/{candidateId}")]
        public async Task<APIResponse> GetCandidateFullProfile(int candidateId)
        {
            try
            {
                return await _unitOfWork.CandidateRepository.GetCandidateFullProfile(candidateId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve candidate profile. Please try again later." };
            }
        }

        [HttpPost("CreateCandidate")]
        public async Task<APIResponse> CreateCandidate([FromForm] Candidate model, IFormFile? resumeFile)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Phone))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate email and phone are required." };

                if (resumeFile != null && resumeFile.Length > 0)
                {
                    model.ResumeFileHash = await ComputeFileHashAsync(resumeFile);
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(resumeFile, "uploads/candidate-resume", model.ResumeUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Resume upload failed. Please try again later." };
                    model.ResumeUrl = fileUrl;
                }

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.CandidateRepository.CreateCandidate(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create candidate. Please try again later." };
            }
        }

        [HttpPost("UpdateCandidate")]
        public async Task<APIResponse> UpdateCandidate([FromForm] Candidate model, IFormFile? resumeFile)
        {
            try
            {
                if (model == null || model.CandidateId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid candidate." };

                if (resumeFile != null && resumeFile.Length > 0)
                {
                    model.ResumeFileHash = await ComputeFileHashAsync(resumeFile);
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(resumeFile, "uploads/candidate-resume", model.ResumeUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Resume upload failed. Please try again later." };
                    model.ResumeUrl = fileUrl;
                }

                return await _unitOfWork.CandidateRepository.UpdateCandidate(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update candidate. Please try again later." };
            }
        }

        [HttpDelete("DeleteCandidate")]
        public async Task<APIResponse> DeleteCandidate(DeleteRecordVM model)
        {
            try
            {
                return await _unitOfWork.CandidateRepository.DeleteCandidate(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete candidate. Please try again later." };
            }
        }

        [HttpPost("CheckDuplicateCandidate")]
        public async Task<APIResponse> CheckDuplicateCandidate([FromBody] CheckDuplicateCandidateRequest model)
        {
            try
            {
                return await _unitOfWork.CandidateRepository.CheckDuplicateCandidate(model.Email, model.Phone, model.ResumeFileHash);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to check duplicate candidate. Please try again later." };
            }
        }

        private static async Task<string> ComputeFileHashAsync(IFormFile file)
        {
            using var sha256 = SHA256.Create();
            using var stream = file.OpenReadStream();
            var hashBytes = await Task.Run(() => sha256.ComputeHash(stream));
            return Convert.ToHexString(hashBytes);
        }
    }

    public class CheckDuplicateCandidateRequest
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ResumeFileHash { get; set; }
    }
}
