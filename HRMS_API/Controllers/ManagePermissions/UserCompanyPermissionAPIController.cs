using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManagePermissions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCompanyPermissionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCompanyPermissionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<APIResponse> GetByEmployeeId(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.UserCompanyPermissionsRepository.GetUserCompanyPermissionsByEmployeeId(employeeId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }

        [HttpGet("GetAll")]
        public async Task<APIResponse> GetAll()
        {
            try
            {
                var data = await _unitOfWork.UserCompanyPermissionsRepository.GetAllUserCompanyPermissions();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }

        [HttpGet("GetByCompanyId/{companyId}")]
        public async Task<APIResponse> GetByCompanyId(int companyId)
        {
            try
            {
                var data = await _unitOfWork.UserCompanyPermissionsRepository.GetUserCompanyPermissionsByCompanyId(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }

        [HttpPost("Create")]
        public async Task<APIResponse> Create(VMUserCompanyPermission model)
        {
            try
            {
                if (model == null || model.EmployeeId is null or 0 || model.CompanyId is null or 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee and Company are required." };

                model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.UserCompanyPermissionsRepository.CreateUserCompanyPermissions(model);

                return new APIResponse { isSuccess = result.Success == 1, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while creating record." };
            }
        }

        [HttpPut("Update")]
        public async Task<APIResponse> Update(VMUserCompanyPermission model)
        {
            try
            {
                if (model == null || model.UserCompanyPermissionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid company permission details." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.UserCompanyPermissionsRepository.UpdateUserCompanyPermissions(model);

                return new APIResponse { isSuccess = result.Success == 1, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while updating record." };
            }
        }

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid delete request." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.UserCompanyPermissionsRepository.DeleteUserCompanyPermissions(model);

                return new APIResponse { isSuccess = result.Success == 1, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while deleting record." };
            }
        }
    }
}
