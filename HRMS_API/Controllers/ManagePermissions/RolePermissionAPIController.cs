using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolePermissionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllRolesWithPermissionByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllRolesWithPermissionByCompanyId(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.RolePermissionRepository.GetAllRolesWithPermissionByCompanyId(CompanyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


        [HttpPost("CreateRolePermission")]
        public async Task<APIResponse> CreateRolePermission(RolePermission permission)

        {
            try
            {
                if (permission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Role Permission details cannot be null." };

                    var addedResult = await _unitOfWork.RolePermissionRepository.CreateRolePermission(permission);
                

                return new APIResponse { isSuccess = true, ResponseMessage = "Role and permission has been added successfully" };
            }
            catch (Exception ex)
            {
                // This will catch unexpected runtime exceptions
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Exception during role creation: {ex.Message}"
                };
            }
        }

        [HttpPost("GetAllPermissionByRoleId")]
        public async Task<APIResponse> GetAllPermissionByRoleId(vmRoleManagePermission vmRole)
        {
            try
            {
                var data = await _unitOfWork.RolePermissionRepository.GetAllPermissionByRoleId(vmRole);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpPost("GetAllRolesWithPermissionByRoleId")]
        public async Task<APIResponse> GetAllRolesWithPermissionByRoleId(vmRoleManagePermission vmRole)
        {
            try
            {
                var data = await _unitOfWork.RolePermissionRepository.GetAllRolesWithPermissionByRoleId(vmRole);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


       

    }
}
