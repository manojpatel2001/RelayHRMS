using HRMS_API.Services;
using HRMS_Core.Recruitment;
using HRMS_Core.Services;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OfferApprovalService _offerApprovalService;
        private readonly FileUploadService _fileUploadService;
        private readonly EmailService _emailService;

        public OfferAPIController(IUnitOfWork unitOfWork, OfferApprovalService offerApprovalService, FileUploadService fileUploadService, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _offerApprovalService = offerApprovalService;
            _fileUploadService = fileUploadService;
            _emailService = emailService;
        }

        [HttpGet("GetAllOffers")]
        public async Task<APIResponse> GetAllOffers(int? companyId)
        {
            try { return await _unitOfWork.OfferRepository.GetAllOffers(companyId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve offers. Please try again later." }; }
        }

        [HttpGet("GetOfferById/{offerId}")]
        public async Task<APIResponse> GetOfferById(int offerId)
        {
            try { return await _unitOfWork.OfferRepository.GetOfferById(offerId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve offer. Please try again later." }; }
        }

        [HttpGet("GetOfferByCandidateApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetOfferByCandidateApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.OfferRepository.GetOfferByCandidateApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve offer. Please try again later." }; }
        }

        [HttpPost("CreateOffer")]
        public async Task<APIResponse> CreateOffer([FromBody] Offer model)
        {
            try
            {
                if (model == null || model.CandidateApplicationId == 0 || model.JobPositionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application and job position are required." };

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.OfferRepository.CreateOffer(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create offer. Please try again later." }; }
        }

        [HttpPost("UpdateOffer")]
        public async Task<APIResponse> UpdateOffer([FromBody] Offer model)
        {
            try
            {
                if (model == null || model.OfferId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid offer." };
                return await _unitOfWork.OfferRepository.UpdateOffer(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update offer. Please try again later." }; }
        }

        [HttpDelete("DeleteOffer")]
        public async Task<APIResponse> DeleteOffer(DeleteRecordVM model)
        {
            try { return await _unitOfWork.OfferRepository.DeleteOffer(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete offer. Please try again later." }; }
        }

        // Moves an Approved offer to Released — the missing step between approval and the
        // candidate response (RecordCandidateResponse only applies to Released offers).
        [HttpPut("ReleaseOffer")]
        public async Task<APIResponse> ReleaseOffer([FromBody] ReleaseOfferRequest model)
        {
            try { return await _unitOfWork.OfferRepository.SetOfferStatus(model.OfferId, "Released", model.UpdatedBy); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to release the offer. Please try again later." }; }
        }

        // Kicks off the self-contained Offer Approval chain (see OfferApprovalService).
        [HttpPost("SubmitForApproval")]
        public async Task<APIResponse> SubmitForApproval([FromBody] SubmitOfferApprovalRequest model)
        {
            try
            {
                var (success, message) = await _offerApprovalService.CreateApprovalRequestAsync(model.OfferId, model.CreatedBy);
                return new APIResponse { isSuccess = success, ResponseMessage = message };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to submit offer for approval. Please try again later.", Data = ex.Message };
            }
        }

        // Stores the client-generated offer letter PDF (html2canvas + jsPDF, same pipeline
        // as Views\Letter\OfferLetter.cshtml) via the existing FileUploadService.
        [HttpPost("UploadOfferLetter")]
        public async Task<APIResponse> UploadOfferLetter(IFormFile letterFile, [FromForm] int offerId, [FromForm] int? updatedBy)
        {
            try
            {
                if (letterFile == null || letterFile.Length == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Offer letter file is required." };

                var offerResponse = await _unitOfWork.OfferRepository.GetOfferById(offerId);
                var existing = (offerResponse?.Data as Offer)?.OfferLetterUrl;

                var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(letterFile, "uploads/offer-letter", existing);
                if (string.IsNullOrEmpty(fileUrl))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Offer letter upload failed. Please try again later." };

                return await _unitOfWork.OfferRepository.SetOfferLetterUrl(offerId, fileUrl, updatedBy);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save offer letter. Please try again later." }; }
        }

        // Emails the offer letter PDF to the candidate — same base64-in/PdfFile-out pipeline
        // as ProbationConfirmation.cshtml's SendProbationConfirmationEmail flow.
        [HttpPost("SendOfferLetterEmail")]
        public async Task<APIResponse> SendOfferLetterEmail([FromBody] SendOfferLetterEmailRequest model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.PdfBase64) || string.IsNullOrWhiteSpace(model.CandidateEmail))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate email and offer letter PDF are required." };

                byte[] pdfBytes = Convert.FromBase64String(model.PdfBase64);

                var emailRequest = new EmailRequest
                {
                    ToEmails = new List<string> { model.CandidateEmail },
                    Subject = $"Offer Letter - {model.PositionTitle}",
                    TemplateName = "OfferLetterEmailTemplate.html",
                    Placeholders = new Dictionary<string, string>
                    {
                        { "CandidateName", model.CandidateName ?? "Candidate" },
                        { "Designation", model.PositionTitle ?? "N/A" },
                        { "CTC", model.OfferedCTC ?? "N/A" },
                        { "JoiningDate", model.JoiningDate ?? "N/A" },
                        { "CompanyName", "Relay Express" }
                    },
                    PdfFile = pdfBytes
                };

                var sent = await _emailService.SendEmailAsync(emailRequest);
                return new APIResponse { isSuccess = sent, ResponseMessage = sent ? "Offer letter emailed successfully!" : "Failed to send offer letter email." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to send offer letter email. Please try again later.", Data = ex.Message };
            }
        }

        // Records the candidate's response to a released offer (HR-recorded — no public
        // candidate portal in this phase). Advances the Candidate Pipeline via Phase 1's
        // existing sp_MoveCandidatePipelineStage-backed endpoint on a successful Accept.
        [HttpPut("RecordCandidateResponse")]
        public async Task<APIResponse> RecordCandidateResponse([FromBody] RecordCandidateResponseRequest model)
        {
            try
            {
                var result = await _unitOfWork.OfferRepository.RecordCandidateResponse(model.OfferId, model.Status, model.AcceptedByCandidateName, model.DeclineReason, model.UpdatedBy);

                if (result.isSuccess && string.Equals(model.Status, "Accepted", StringComparison.OrdinalIgnoreCase) && model.AcceptedPipelineStageId.HasValue)
                {
                    var offerResponse = await _unitOfWork.OfferRepository.GetOfferById(model.OfferId);
                    if (offerResponse?.Data is Offer offer)
                    {
                        await _unitOfWork.CandidatePipelineRepository.MoveCandidateToStage(
                            offer.CandidateApplicationId, model.AcceptedPipelineStageId.Value, "Offer accepted by candidate.", model.UpdatedBy);
                    }
                }

                return result;
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to record candidate response. Please try again later." }; }
        }
    }

    public class SubmitOfferApprovalRequest
    {
        public int OfferId { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class ReleaseOfferRequest
    {
        public int OfferId { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class SendOfferLetterEmailRequest
    {
        public int OfferId { get; set; }
        public string PdfBase64 { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public string? CandidateName { get; set; }
        public string? PositionTitle { get; set; }
        public string? OfferedCTC { get; set; }
        public string? JoiningDate { get; set; }
    }

    public class RecordCandidateResponseRequest
    {
        public int OfferId { get; set; }
        public string Status { get; set; } = string.Empty; // Accepted | Declined
        public string? AcceptedByCandidateName { get; set; }
        public string? DeclineReason { get; set; }
        public int? AcceptedPipelineStageId { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
