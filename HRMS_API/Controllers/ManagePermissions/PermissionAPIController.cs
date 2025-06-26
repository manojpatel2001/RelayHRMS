using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.ManagePermissions;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{

    [Route("api/[controller]")]
    [ApiController]
    public class PermissionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllPermissions")]
        public async Task<APIResponse> GetAllPermissions()
        {
            try
            {
                var data = await _unitOfWork.PermissionRepository.GetAllPermissions();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetPermissionById/{PermissionId}")]
        public async Task<APIResponse> GetPermissionById(int PermissionId)
        {
            try
            {
                var vmCommonGetById = new vmCommonGetById { Id = PermissionId, IsDeleted = false, IsEnabled = true };
                var data = await _unitOfWork.PermissionRepository.GetPermissionById(vmCommonGetById);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreatePermission")]
        public async Task<APIResponse> CreatePermission(Permission model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Permission details cannot be null." };
                var existPermissionName = await _unitOfWork.PermissionRepository.GetAllAsync(x => x.PermissionName.ToLower() == model.PermissionName.ToLower() && x.IsDeleted == false && x.IsEnabled == true);
                if (existPermissionName.Any())
                {
                    return new APIResponse { isSuccess=false,ResponseMessage=$"Record with permission name '{model.PermissionName}' already added "};
                }
                var existSlug = await _unitOfWork.PermissionRepository.GetAllAsync(x => x.Slug.ToLower() == model.Slug.ToLower() && x.IsDeleted == false && x.IsEnabled == true);
                if (existSlug.Any())
                {
                    return new APIResponse { isSuccess=false,ResponseMessage=$"Record with permission name '{model.Slug}' already added "};
                }
                    
                    model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PermissionRepository.CreatePermission(model);

                if (result.Id > 0)
                {
                    var newPermission = await _unitOfWork.PermissionRepository.GetPermissionById(new vmCommonGetById { Id = result.Id, IsDeleted = false, IsEnabled = true });
                    return new APIResponse { isSuccess = true, Data = newPermission, ResponseMessage = "The record has been saved successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdatePermission")]
        public async Task<APIResponse> UpdatePermission(Permission permission)
        {
            try
            {
                if (permission == null || permission.PermissionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Permission details cannot be null." };

                var checkPermission = await _unitOfWork.PermissionRepository.GetPermissionById(new vmCommonGetById { Id = permission.PermissionId, IsDeleted = false, IsEnabled = true });
                if (checkPermission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var existPermissionName = await _unitOfWork.PermissionRepository.GetAllAsync(x => x.PermissionId != permission.PermissionId && x.PermissionName.ToLower() == permission.PermissionName.ToLower() && x.IsDeleted == false && x.IsEnabled == true);
                if (existPermissionName.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with permission name '{permission.PermissionName}' already added " };
                }
                var existSlug = await _unitOfWork.PermissionRepository.GetAllAsync(x => x.PermissionId != permission.PermissionId && x.Slug.ToLower() == permission.Slug.ToLower() && x.IsDeleted == false && x.IsEnabled == true);
                if (existSlug.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with permission name '{permission.Slug}' already added " };
                }
                permission.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PermissionRepository.UpdatePermission(permission);

                if (result.Id > 0)
                {
                    var updatedPermission = await _unitOfWork.PermissionRepository.GetPermissionById(new vmCommonGetById { Id = permission.PermissionId, IsDeleted = false, IsEnabled = true });
                    return new APIResponse { isSuccess = true, Data = updatedPermission, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeletePermission")]
        public async Task<APIResponse> DeletePermission(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var checkPermission = await _unitOfWork.PermissionRepository.GetPermissionById(new vmCommonGetById { Id = model.Id, IsDeleted = false, IsEnabled = true });
                if (checkPermission == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PermissionRepository.DeletePermission(model);

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
