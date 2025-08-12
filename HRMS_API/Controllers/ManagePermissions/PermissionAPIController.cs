using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.ManagePermissions;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
             

                var result = await _unitOfWork.PermissionRepository.CreatePermission(model);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true,  ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
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

                
                var result = await _unitOfWork.PermissionRepository.UpdatePermission(permission);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };


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

                
                var result = await _unitOfWork.PermissionRepository.DeletePermission(model);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)

            {
                return new APIResponse { isSuccess = false,  ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetAllGroupPermissions/{PermissionType}")]
        public async Task<APIResponse> GetAllGroupPermissions(string PermissionType)
        {
            try
            {
                var actionOrder = new List<string> { "View", "Add", "Edit", "Delete", "Block" };

                var data = await _unitOfWork.PermissionRepository.GetAllGroupPermissionList(PermissionType);

                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                var groupData = data
                    .GroupBy(g => g.GroupName)
                    .Select(g => new
                    {
                        GroupName = g.Key,
                        PermissionRoleTypeName = g.Select(x => x.PermissionRoleTypeName).FirstOrDefault(),
                        Permissions = g
                            .Select(x => new
                            {
                                x.PermissionId,
                                PermissionName = x.FirstPermissionName,
                                x.Slug,
                            })
                            .OrderBy(p =>
                                actionOrder.IndexOf(p.PermissionName) >= 0
                                    ? actionOrder.IndexOf(p.PermissionName)
                                    : int.MaxValue // Put unknown actions at the end
                            )
                            .ToList()
                    })
                    .ToList();

                return new APIResponse { isSuccess = true, Data = groupData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

    }
}
