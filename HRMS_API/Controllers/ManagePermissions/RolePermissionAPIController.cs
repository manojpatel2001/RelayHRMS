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
        public async Task<APIResponse> CreateRolePermission([FromBody]RolePermission permission)
        {
            try
            {
                if (permission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Role Permission details cannot be null." };

                    var addedResult = await _unitOfWork.RolePermissionRepository.CreateRolePermission(permission);

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

        [HttpGet("GetAllUserAndRolePermissionList/{EmployeeId}")]
        public async Task<APIResponse> GetAllUserAndRolePermissionList(int EmployeeId)
        {
            try
            {
                var roleAndPermission = await _unitOfWork.RolePermissionRepository.GetEmployeeRolesAndPermissions(EmployeeId);

                if (roleAndPermission == null || !roleAndPermission.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                var permssions = new List<string>();
                foreach (var role in roleAndPermission)
                {
                    if (!String.IsNullOrEmpty(role.PermissionSlug))
                    {
                        permssions.Add(role.PermissionSlug);
                    }
                }
                return new APIResponse { isSuccess = true, Data = permssions, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
    }
}
