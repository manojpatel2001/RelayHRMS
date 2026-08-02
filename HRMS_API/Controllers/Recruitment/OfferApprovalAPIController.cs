using HRMS_API.Services;
using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferApprovalAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OfferApprovalService _offerApprovalService;

        public OfferApprovalAPIController(IUnitOfWork unitOfWork, OfferApprovalService offerApprovalService)
        {
            _unitOfWork = unitOfWork;
            _offerApprovalService = offerApprovalService;
        }

        [HttpGet("GetLevelConfig")]
        public async Task<APIResponse> GetLevelConfig(int? companyId)
        {
            try { return await _unitOfWork.OfferApprovalRepository.GetLevelConfig(companyId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve approval configuration. Please try again later." }; }
        }

        [HttpPost("SaveLevelConfig")]
        public async Task<APIResponse> SaveLevelConfig([FromBody] OfferApprovalLevelConfig model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.ApproverRole))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Approver role is required." };

                return model.OfferApprovalLevelConfigId > 0
                    ? await _unitOfWork.OfferApprovalRepository.UpdateLevelConfig(model)
                    : await _unitOfWork.OfferApprovalRepository.CreateLevelConfig(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save approval configuration. Please try again later." }; }
        }

        [HttpDelete("DeleteLevelConfig/{offerApprovalLevelConfigId}")]
        public async Task<APIResponse> DeleteLevelConfig(int offerApprovalLevelConfigId, int? deletedBy)
        {
            try { return await _unitOfWork.OfferApprovalRepository.DeleteLevelConfig(offerApprovalLevelConfigId, deletedBy); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to remove approval level. Please try again later." }; }
        }

        [HttpGet("GetRequestByOfferId/{offerId}")]
        public async Task<APIResponse> GetRequestByOfferId(int offerId)
        {
            try { return await _unitOfWork.OfferApprovalRepository.GetRequestByOfferId(offerId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve approval request. Please try again later." }; }
        }

        [HttpGet("GetRequestLevelsByRequestId/{offerApprovalRequestId}")]
        public async Task<APIResponse> GetRequestLevelsByRequestId(int offerApprovalRequestId)
        {
            try { return await _unitOfWork.OfferApprovalRepository.GetRequestLevelsByRequestId(offerApprovalRequestId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve approval levels. Please try again later." }; }
        }

        [HttpGet("GetPendingApprovals/{approverEmployeeId}")]
        public async Task<APIResponse> GetPendingApprovals(int approverEmployeeId)
        {
            try { return await _unitOfWork.OfferApprovalRepository.GetPendingApprovalsByApproverEmployeeId(approverEmployeeId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve pending approvals. Please try again later." }; }
        }

        [HttpGet("GetRequestHistory/{offerApprovalRequestId}")]
        public async Task<APIResponse> GetRequestHistory(int offerApprovalRequestId)
        {
            try { return await _unitOfWork.OfferApprovalRepository.GetRequestHistoryByRequestId(offerApprovalRequestId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve approval history. Please try again later." }; }
        }

        [HttpPut("ActionOnLevel")]
        public async Task<APIResponse> ActionOnLevel([FromBody] ActionOnOfferApprovalLevelRequest model)
        {
            try
            {
                var (success, message) = await _offerApprovalService.ActionOnLevelAsync(model.OfferApprovalRequestLevelId, model.Action, model.ActionBy, model.Remarks);
                return new APIResponse { isSuccess = success, ResponseMessage = message };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to record approval action. Please try again later.", Data = ex.Message };
            }
        }
    }

    public class ActionOnOfferApprovalLevelRequest
    {
        public int OfferApprovalRequestLevelId { get; set; }
        public string Action { get; set; } = string.Empty; // Approve | Reject
        public int ActionBy { get; set; }
        public string? Remarks { get; set; }
    }
}
