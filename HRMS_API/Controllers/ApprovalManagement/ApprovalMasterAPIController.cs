using HRMS_Core.VM.ApprovalManagement;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ApprovalManagement
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("ManageApprovalMaster")]
        public async Task<APIResponse> ManageApprovalMaster([FromBody] ApprovalMasterViewModel model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Approval details cannot be null." };

                var result = await _unitOfWork.ApprovalMasterRepository.ManageApprovalMaster(model);
                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to manage approval master ."
                };
            }
        }

        [HttpGet("GetAllApprovalMasters")]
        public async Task<APIResponse> GetAllApprovalMasters()
        {
            try
            {
                var result = await _unitOfWork.ApprovalMasterRepository.GetAllApprovalMasters();
                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch approvals."
                };
            }
        }

        [HttpGet("GetAllApprovalMasterList")]
        public async Task<APIResponse> GetAllApprovalMasterList(int? ApprovalTypeId=null )
        {
            try
            {
                var result = await _unitOfWork.ApprovalMasterRepository.GetAllApprovalMasterList(ApprovalTypeId);
                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch approvals ."
                };
            }
        }

        [HttpGet("GetAllApprovalMasterType")]
        public async Task<APIResponse> GetAllApprovalMasterType()
        {
            try
            {
                var result = await _unitOfWork.ApprovalMasterRepository.GetAllApprovalMasterType();
                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch approval types ."
                };
            }
        }
    }
}
