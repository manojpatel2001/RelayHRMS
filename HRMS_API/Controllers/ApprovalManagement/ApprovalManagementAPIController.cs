using HRMS_Core.VM.ApprovalManagement;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ApprovalManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalManagementAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalManagementAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -------------------------------------------------------------
        // 1️⃣ MANAGE APPROVAL SCHEME LEVEL
        // -------------------------------------------------------------
        [HttpPost("ManageApprovalSchemeLevel")]
        public async Task<APIResponse> ManageApprovalSchemeLevel([FromBody] List<ApprovalSchemeLevelVM> model)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.ManageApprovalSchemeLevel(model);
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = ex.Message // Return the actual error for debugging
                };
            }
        }



        // -------------------------------------------------------------
        // 2️⃣ MANAGE APPROVAL REQUEST
        // -------------------------------------------------------------
        [HttpPost("ManageApprovalRequest")]
        public async Task<APIResponse> ManageApprovalRequest(ApprovalRequestVM model)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.ManageApprovalRequest(model);
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to process approval request."
                };
            }
        }


        // -------------------------------------------------------------
        // 3️⃣ MANAGE APPROVAL REQUEST LEVEL
        // -------------------------------------------------------------
        [HttpPost("ManageApprovalRequestLevel")]
        public async Task<APIResponse> ManageApprovalRequestLevel(ApprovalRequestLevelVM model)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.ManageApprovalRequestLevel(model);
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to process approval request level."
                };
            }
        }


        // -------------------------------------------------------------
        // 4️⃣ INSERT APPROVAL REQUEST HISTORY
        // -------------------------------------------------------------
        [HttpPost("InsertApprovalRequestHistory")]
        public async Task<APIResponse> InsertApprovalRequestHistory(ApprovalRequestHistoryVM model)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.InsertApprovalRequestHistory(model);
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to insert approval request history."
                };
            }
        }


        // -------------------------------------------------------------
        // 5️⃣ ACTION ON APPROVAL REQUEST LEVEL (APPROVE / REJECT)
        // -------------------------------------------------------------
        [HttpPost("ActionOnApprovalRequestLevel")]
        public async Task<APIResponse> ActionOnApprovalRequestLevel(ApprovalRequestLevelActionVM model)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.ActionOnApprovalRequestLevel(model);
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to take action on approval request level."
                };
            }
        }


        // -------------------------------------------------------------
        // 6️⃣ GET LIST — APPROVAL SCHEME LEVEL
        // -------------------------------------------------------------
        [HttpGet("GetApprovalSchemeLevels")]
        public async Task<APIResponse> GetApprovalSchemeLevels()
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetApprovalSchemeLevels();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result
                };
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to retrieve approval scheme levels."
                };
            }
        }


        // -------------------------------------------------------------
        // 7️⃣ GET LIST — APPROVAL REQUESTS
        // -------------------------------------------------------------
        [HttpGet("GetApprovalRequests")]
        public async Task<APIResponse> GetApprovalRequests()
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetApprovalRequests();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result
                };
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to retrieve approval requests."
                };
            }
        }


        // -------------------------------------------------------------
        // 8️⃣ GET LIST — REQUEST LEVELS
        // -------------------------------------------------------------
        [HttpGet("GetApprovalRequestLevels/{requestId}")]
        public async Task<APIResponse> GetApprovalRequestLevels(int requestId)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetApprovalRequestLevels(requestId);

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result
                };
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to retrieve approval request levels."
                };
            }
        }


        // -------------------------------------------------------------
        // 9️⃣ GET LIST — REQUEST HISTORY
        // -------------------------------------------------------------
        [HttpGet("GetApprovalRequestHistory/{requestId}")]
        public async Task<APIResponse> GetApprovalRequestHistory(int requestId)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetApprovalRequestHistory(requestId);

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result
                };
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to retrieve approval request history."
                };
            }
        }
        [HttpGet("GetAllApprovalSchemeLevelsByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllApprovalSchemeLevelsByCompanyId(int CompanyId)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetAllApprovalSchemeLevelsByCompanyId(CompanyId);

                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to retrieve approval request history."
                };
            }
        }
        [HttpGet("GetAllApprovalSchemeLevelsBySchemeId/{SchemeId}")]
        public async Task<APIResponse> GetAllApprovalSchemeLevelsBySchemeId(int SchemeId)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetAllApprovalSchemeLevelsBySchemeId(SchemeId);

                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to retrieve approval request history."
                };
            }
        }

        [HttpPost("DeleteApprovalSchemeLevel")]
        public async Task<APIResponse> DeleteApprovalSchemeLevel(ApprovalSchemeLevelPara model)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.DeleteApprovalSchemeLevel(model);

                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to delete approval ."
                };
            }
        }
    }
}
