using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleAPIController : ControllerBase
    {
        private readonly RoleManager<HRMSRoleIdentity> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleAPIController(RoleManager<HRMSRoleIdentity> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllRoles")]
        public async Task<APIResponse> GetAllRoles()
        {
            try
            {
                var data = await _unitOfWork.RoleRepository.GetAllRoles();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


        [HttpPost("CreateRole")]
        public async Task<APIResponse> CreateRole(vmRole vmRole)
        {
            try
            {
                if (vmRole == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Role details cannot be null." };

                var roleExists = await _roleManager.RoleExistsAsync(vmRole.RoleName);
                if (roleExists)
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Role name {vmRole.RoleName} already exist." };

                var existSlug = await _unitOfWork.RoleRepository.GetAllAsync(x => x.Slug == vmRole.Slug && x.IsDeleted == false && x.IsEnabled == true);
                if(existSlug.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Slug name {vmRole.Slug} already exist." };

                var newRoleData = new HRMSRoleIdentity
                {
                    Name = vmRole.RoleName,
                    Description = vmRole.Description,
                    Slug = vmRole.Slug,
                };
                var result = await _roleManager.CreateAsync(newRoleData);

                if (!result.Succeeded)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable add role,Please try again" };

                }

                return new APIResponse { isSuccess = true, ResponseMessage = "Role has been added successfully" };
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


        [HttpPut("UpdateRole")]
        public async Task<APIResponse> UpdateRole(vmRole vmRole)
        {
            try
            {
                if (vmRole == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Role details cannot be null." };


                var existingRole = await _roleManager.FindByIdAsync(vmRole.RoleId.ToString());
                if (existingRole == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Role not found." };

                var roleNameExists = await _unitOfWork.RoleRepository.GetAllAsync(x => x.Id != vmRole.RoleId && x.Name == vmRole.RoleName && x.IsDeleted == false && x.IsEnabled == true);
                if (roleNameExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Role name {vmRole.RoleName} already exist." };

                var existSlug = await _unitOfWork.RoleRepository.GetAllAsync(x => x.Id != vmRole.RoleId && x.Slug == vmRole.Slug && x.IsDeleted == false && x.IsEnabled == true);
                if (existSlug.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Slug name {vmRole.Slug} already exist." };



                existingRole.Id = vmRole.RoleId;
                existingRole.Name = vmRole.RoleName;
                existingRole.Description = vmRole.Description;
                existingRole.Slug = vmRole.Slug;
                var result = await _roleManager.UpdateAsync(existingRole);

                if (!result.Succeeded)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable add role,Please try again" };

                }
           

                return new APIResponse { isSuccess = true, ResponseMessage = "Role has been updated successfully" };
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

        [HttpDelete("DeleteRole")]
        public async Task<APIResponse> DeleteRole(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };


                var role = await _roleManager.FindByIdAsync(model.Id.ToString());
                if (role == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Role not found."
                    };
                }

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Failed to delete role."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Role deleted permanently."
                };


               
            }
            catch (Exception ex)

            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }


    }
}
