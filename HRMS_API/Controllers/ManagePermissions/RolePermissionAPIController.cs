using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
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
        private readonly RoleManager<HRMSRoleIdentity> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RolePermissionAPIController(RoleManager<HRMSRoleIdentity> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateRolePermission")]
        public async Task<APIResponse> CreateRolePermission(vmPermisionRole vmPermisionRole)
        {
            try
            {
                if (vmPermisionRole == null)
                return new APIResponse { isSuccess = false, ResponseMessage = "Role Permission details cannot be null." };

                var roleExists = await _roleManager.RoleExistsAsync(vmPermisionRole.RoleName);
                if (roleExists)
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Role name {vmPermisionRole.RoleName} already exist." };
                var newRoleData = new HRMSRoleIdentity
                { 
                    Name=vmPermisionRole.RoleName,
                    Description=vmPermisionRole.Description,
                    Slug=vmPermisionRole.Slug,
                };
                var result = await _roleManager.CreateAsync(newRoleData);

                if (!result.Succeeded)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable add role,Please try again" };

                }

                foreach (var permissionId in vmPermisionRole.PermissionIds)
                {
                    var newPermission = new RolePermission
                    {
                        RoleId = newRoleData.Id,
                        PermissionId = permissionId
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


    }
}
