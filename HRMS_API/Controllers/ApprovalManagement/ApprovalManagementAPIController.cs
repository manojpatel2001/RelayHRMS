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

        
        [HttpGet("GetApprovalDropdownDetails")]
        public async Task<APIResponse> GetApprovalDropdownDetails()
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.GetApprovalDropdownDetails();
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
        [HttpGet("GetAllEmployByDepartmentId")]
        public async Task<APIResponse> GetAllEmployByDepartmentId(int?CompanyId=null,int?DepartmentId=null)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.GetAllEmployByDepartmentId(CompanyId, DepartmentId);
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

        [HttpPost("ManageApprovalLevel")]
        public async Task<APIResponse> ManageApprovalLevel([FromBody] List<ApprovalLevelVM> model)
        {
            try
            {
                return await _unitOfWork.ApprovalManagementRepository.ManageApprovalLevel(model);
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


        [HttpGet("GetAllApprovalLevelsByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllApprovalLevelsByCompanyId(int CompanyId)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetAllApprovalLevelsByCompanyId(CompanyId);

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
        [HttpGet("GetAllApprovalLevelsByApprovalMasterId/{ApprovalMasterId}")]
        public async Task<APIResponse> GetAllApprovalLevelsByApprovalMasterId(int ApprovalMasterId)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetAllApprovalLevelsByApprovalMasterId(ApprovalMasterId);

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

        [HttpPost("DeleteApprovalLevel")]
        public async Task<APIResponse> DeleteApprovalLevel(ApprovalLevelPara model)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.DeleteApprovalLevel(model);

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
        [HttpPost("GetPendingApprovalRequests")]
        public async Task<APIResponse> GetPendingApprovalRequests([FromBody]GetPendingApprovalRequestsPara model)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetPendingApprovalRequests(model);

                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch pending approval request."
                };
            }
        }
        [HttpPost("GetUpcomingProbationDetails")]
        public async Task<APIResponse> GetUpcomingProbationDetails(GetUpcomingProbationDetailsPara model)
        {
            try
            {
                var result = await _unitOfWork.ApprovalManagementRepository.GetUpcomingProbationDetails(model);

                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch up comming probation ."
                };
            }
        }
    }
}
