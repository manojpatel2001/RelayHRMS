using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePersonalInfoAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeePersonalInfoAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmployeePersonalInfo")]
        public async Task<APIResponse> GetAllEmployeePersonalInfo()
        {
            try
            {
                var data = await _unitOfWork.EmployeePersonalInfoRepository.GetAllEmployeePersonalInfo();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetEmployeePersonalInfoByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeePersonalInfoByEmployeeId(string EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoByEmployeeId(EmployeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetEmployeePersonalInfoById/{id}")]
        public async Task<APIResponse> GetEmployeePersonalInfoById(int id)
        {
            try
            {
                var data = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateEmployeePersonalInfo")]
        public async Task<APIResponse> CreateEmployeePersonalInfo(EmployeePersonalInfo model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee personal info details cannot be null." };

                if (model.EmployeePersonalInfoId == 0)
                {
                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.EmployeePersonalInfoRepository.CreateEmployeePersonalInfo(model);

                    if (result.Id > 0)
                    {
                        var newEmployeePersonalInfo = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newEmployeePersonalInfo, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(model.EmployeePersonalInfoId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid employee personal info record." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.EmployeePersonalInfoRepository.UpdateEmployeePersonalInfo(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(model.EmployeePersonalInfoId);
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

        [HttpPut("UpdateEmployeePersonalInfo")]
        public async Task<APIResponse> UpdateEmployeePersonalInfo(EmployeePersonalInfo model)
        {
            try
            {
                if (model == null || model.EmployeePersonalInfoId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee personal info details cannot be null." };

                var check = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(model.EmployeePersonalInfoId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid employee personal info record." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmployeePersonalInfoRepository.UpdateEmployeePersonalInfo(model);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(model.EmployeePersonalInfoId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeePersonalInfo")]
        public async Task<APIResponse> DeleteEmployeePersonalInfo(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid employee personal info record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmployeePersonalInfoRepository.DeleteEmployeePersonalInfo(model);

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
