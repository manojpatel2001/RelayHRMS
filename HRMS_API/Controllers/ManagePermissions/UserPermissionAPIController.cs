using HRMS_Core.ManagePermission;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserPermissionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateUserPermission")]
        public async Task<APIResponse> CreateUserPermission([FromBody] UserPermission permission)
        {
            try
            {
                if (permission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Role Permission details cannot be null." };

                var addedResult = await _unitOfWork.UserPermissionRepository.CreateUserPermission(permission);

                if (addedResult.Id > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = "Permission has been assigned successfully" };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong!" };
            }
            catch (Exception ex)
            {
                // This will catch unexpected runtime exceptions
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Some thing went wrong!"
                };
            }
        }


        [HttpGet("GetAllEmployeeListByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllEmployeeListByCompanyId(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.UserPermissionRepository.GetAllEmployeeListByCompanyId(CompanyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetAllRolesWithPermissionByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllRolesWithPermissionByCompanyId(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.UserPermissionRepository.GetAllUserWithPermissionByCompanyId(CompanyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("GetAllPermissionByEmployeeId")]
        public async Task<APIResponse> GetAllPermissionByEmployeeId(vmRoleManagePermission vmRole)
        {
            try
            {
                var data = await _unitOfWork.UserPermissionRepository.GetAllPermissionByEmployeeId(vmRole);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpDelete("DeleteUserPermission")]
        public async Task<APIResponse> DeleteUserPermission(vmRoleManagePermission model)
        {
            try
            {
                if (model == null || model.EmployeeId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };


                var result = await _unitOfWork.UserPermissionRepository.DeleteUserPermission(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)

            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        
    }
}
