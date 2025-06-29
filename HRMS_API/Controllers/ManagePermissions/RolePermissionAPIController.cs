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
        public async Task<APIResponse> CreateRolePermission(vmPermisionRole vmPermisionRole)
        
        {
            try
            {
                if (vmPermisionRole == null)
                return new APIResponse { isSuccess = false, ResponseMessage = "Role Permission details cannot be null." };

                var roleExists = await _unitOfWork.RoleRepository.GetAllAsync(x=>x.Id==vmPermisionRole.RoleId);
                if (!roleExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Role cannot found" };

                var permissionExists = await _unitOfWork.RolePermissionRepository.GetAllAsync(x => x.RoleId == vmPermisionRole.RoleId &&x.IsDeleted==false &&x.IsEnabled==true);
                if (permissionExists.Any())
                {
                    var deleteData = new vmRoleManagePermission { RoleId= vmPermisionRole.RoleId,CompanyId= vmPermisionRole .CompanyId};
                    var deleteRolePermission = await _unitOfWork.RolePermissionRepository.DeleteRolePermission(deleteData);
                }

                foreach (var permissionId in vmPermisionRole.PermissionIds)
                {
                    var newPermission = new RolePermission
                    {
                        RoleId = vmPermisionRole.RoleId,
                        PermissionId = permissionId,
                        CompanyId= vmPermisionRole.CompanyId
                    };
                    var addedResult = await _unitOfWork.RolePermissionRepository.CreateRolePermission(newPermission);
                }

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


        [HttpDelete("DeleteRolePermission")]
        public async Task<APIResponse> DeleteRolePermission(vmRoleManagePermission model)
        {
            try
            {
                if (model == null || model.RoleId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var checkPermission = await _unitOfWork.RoleRepository.GetAllAsync(x=>x.Id==model.RoleId&&  x.IsDeleted==false &&x.IsEnabled==true);
                if (checkPermission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var result = await _unitOfWork.RolePermissionRepository.DeleteRolePermission(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)

            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

    }
}
