using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{
    [Route("api/[controller]")]
    [ApiController]
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

                var roleExists = await _unitOfWork.RoleRepository.GetAllAsync(x => x.Name == vmRole.RoleName && x.IsDeleted == false && x.IsEnabled == true);
                if (roleExists.Any())
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

                var roleExists = await _unitOfWork.RoleRepository.GetAllAsync(x =>x.Id!=vmRole.RoleId && x.Name == vmRole.RoleName && x.IsDeleted == false && x.IsEnabled == true);
                if (roleExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Role name {vmRole.RoleName} already exist." };

                var existSlug = await _unitOfWork.RoleRepository.GetAllAsync(x => x.Id != vmRole.RoleId && x.Slug == vmRole.Slug && x.IsDeleted == false && x.IsEnabled == true);
                if (existSlug.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Slug name {vmRole.Slug} already exist." };

                var newRoleData = new HRMSRoleIdentity
                {
                    Id = vmRole.RoleId,
                    Name = vmRole.RoleName,
                    Description = vmRole.Description,
                    Slug = vmRole.Slug,
                };
                var result = await  _unitOfWork.RoleRepository.UpdateRole(newRoleData);

                if (result.Id==0)
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

        [HttpDelete("DeleteRole")]
        public async Task<APIResponse> DeleteRole(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var checkPermission = await _unitOfWork.RoleRepository.GetAsync(x=>x.Id==model.Id && x.IsDeleted==false && x.IsEnabled==true);
                if (checkPermission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.RoleRepository.DeleteRole(model);

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
