using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTypeAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeTypeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmployeeTypes")]
        public async Task<APIResponse> GetAllEmployeeTypes()
        {
            try
            {
                var data = await _unitOfWork.EmployeeTypeRepository.GetAllEmployeeTypes();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeTypeById/{employeeTypeId}")]
        public async Task<APIResponse> GetEmployeeTypeById(int employeeTypeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(employeeTypeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateEmployeeType")]
        public async Task<APIResponse> CreateEmployeeType(EmployeeType model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "EmployeeType details cannot be null." };

                if (model.EmployeeTypeId == 0)
                {
                    var exists = await _unitOfWork.EmployeeTypeRepository.GetAllAsync(et =>
                        et.EmployeeTypeName.ToLower().Trim() == model.EmployeeTypeName.ToLower().Trim() &&
                        et.IsDeleted == false && et.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.EmployeeTypeName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.EmployeeTypeRepository.CreateEmployeeType(model);

                    if (result.Id > 0)
                    {
                        var newEmployeeType = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newEmployeeType, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(model.EmployeeTypeId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid EmployeeType record." };

                    var exists = await _unitOfWork.EmployeeTypeRepository.GetAllAsync(et =>
                        et.EmployeeTypeId != model.EmployeeTypeId &&
                        et.EmployeeTypeName.ToLower().Trim() == model.EmployeeTypeName.ToLower().Trim() &&
                        et.IsDeleted == false && et.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.EmployeeTypeName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.EmployeeTypeRepository.UpdateEmployeeType(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(model.EmployeeTypeId);
                        return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeType")]
        public async Task<APIResponse> UpdateEmployeeType(EmployeeType employeeType)
        {
            try
            {
                if (employeeType == null || employeeType.EmployeeTypeId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "EmployeeType details cannot be null." };

                var check = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(employeeType.EmployeeTypeId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid EmployeeType record." };

                var isExists = await _unitOfWork.EmployeeTypeRepository.GetAllAsync(x =>
                    x.EmployeeTypeId != employeeType.EmployeeTypeId &&
                    x.EmployeeTypeName.ToLower().Trim() == employeeType.EmployeeTypeName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{employeeType.EmployeeTypeName}' already exists." };

                employeeType.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmployeeTypeRepository.UpdateEmployeeType(employeeType);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(employeeType.EmployeeTypeId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update EmployeeType. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeType")]
        public async Task<APIResponse> DeleteEmployeeType(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypeById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid EmployeeType record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmployeeTypeRepository.DeleteEmployeeType(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete EmployeeType. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
