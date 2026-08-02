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
    public class CandidateDocumentAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;

        public CandidateDocumentAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        [HttpGet("GetAllDocumentTypes")]
        public async Task<APIResponse> GetAllDocumentTypes()
        {
            try { return await _unitOfWork.CandidateDocumentRepository.GetAllDocumentTypes(); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve document types. Please try again later." }; }
        }

        [HttpPost("CreateDocumentType")]
        public async Task<APIResponse> CreateDocumentType([FromBody] CandidateDocumentType model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.DocumentName))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Document name is required." };
                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.CandidateDocumentRepository.CreateDocumentType(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create document type. Please try again later." }; }
        }

        [HttpPost("UpdateDocumentType")]
        public async Task<APIResponse> UpdateDocumentType([FromBody] CandidateDocumentType model)
        {
            try
            {
                if (model == null || model.CandidateDocumentTypeId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid document type." };
                return await _unitOfWork.CandidateDocumentRepository.UpdateDocumentType(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update document type. Please try again later." }; }
        }

        [HttpDelete("DeleteDocumentType")]
        public async Task<APIResponse> DeleteDocumentType(DeleteRecordVM model)
        {
            try { return await _unitOfWork.CandidateDocumentRepository.DeleteDocumentType(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete document type. Please try again later." }; }
        }

        [HttpGet("GetDocumentsByApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetDocumentsByApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.CandidateDocumentRepository.GetDocumentsByApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve documents. Please try again later." }; }
        }

        [HttpPost("UploadDocument")]
        public async Task<APIResponse> UploadDocument([FromForm] CandidateDocument model, IFormFile? documentFile)
        {
            try
            {
                if (model == null || model.CandidateApplicationId == 0 || model.CandidateDocumentTypeId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application and document type are required." };

                if (documentFile != null && documentFile.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(documentFile, "uploads/candidate-document", model.DocumentUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Document upload failed. Please try again later." };
                    model.DocumentUrl = fileUrl;
                }

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.CandidateDocumentRepository.CreateDocument(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to upload document. Please try again later." }; }
        }

        [HttpPost("UpdateDocument")]
        public async Task<APIResponse> UpdateDocument([FromForm] CandidateDocument model, IFormFile? documentFile)
        {
            try
            {
                if (model == null || model.CandidateDocumentId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid document." };

                if (documentFile != null && documentFile.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(documentFile, "uploads/candidate-document", model.DocumentUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Document upload failed. Please try again later." };
                    model.DocumentUrl = fileUrl;
                }

                return await _unitOfWork.CandidateDocumentRepository.UpdateDocument(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update document. Please try again later." }; }
        }

        [HttpDelete("DeleteDocument")]
        public async Task<APIResponse> DeleteDocument(DeleteRecordVM model)
        {
            try { return await _unitOfWork.CandidateDocumentRepository.DeleteDocument(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete document. Please try again later." }; }
        }
    }
}
