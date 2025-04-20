using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.CompanyStructure
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarningMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarningMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllWarningMaster")]
        public async Task<APIResponse> GetAllWarningMaster()
        {
            try
            {
                var data = await _unitOfWork.WarningMasterRepository.GetAllWarningMaster();
                if (data == null || !data.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }

        [HttpGet("GetByWarningMasterId/{id}")]
        public async Task<APIResponse> GetByWarningMasterId(int id)
        {
            try
            {
                var data = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(id);
                if (data == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record, Please try again later!" };
            }
        }

        [HttpPost("CreateOrUpdateWarningMaster")]
        public async Task<APIResponse> CreateOrUpdateWarningMaster(WarningMaster warning)
        {
            try
            {
                if (warning == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Warning master details cannot be null" };
                }

                if (warning.WarningMasterId == 0)
                {
                    var isExists = await _unitOfWork.WarningMasterRepository.GetAllAsync(asd => asd.WarningName.ToLower().Trim() == warning.WarningName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{warning.WarningName}' already exists" };
                    }
                    warning.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.WarningMasterRepository.CreateWarningMaster(warning);
                    if (result.Id > 0)
                    {
                        var newData = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "The record has been saved successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record" };
                }
                else
                {
                    var check = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(warning.WarningMasterId);
                    if (check == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                    }
                    var isExists = await _unitOfWork.WarningMasterRepository.GetAllAsync(asd =>asd.WarningMasterId!=warning.WarningMasterId && asd.WarningName.ToLower().Trim() == warning.WarningName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{warning.WarningName}' already exists" };
                    }
                    warning.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.WarningMasterRepository.UpdateWarningMaster(warning);
                    if (result.Id > 0)
                    {
                        var updatedData = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(warning.WarningMasterId);
                        return new APIResponse { isSuccess = true, Data = updatedData, ResponseMessage = "The record has been updated successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add/update record, Please try again later!" };
            }
        }

        [HttpPut("UpdateWarningMaster")]
        public async Task<APIResponse> UpdateWarningMaster(WarningMaster warning)
        {
            try
            {
                if (warning == null || warning.WarningMasterId == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Warning master details cannot be null" };
                }

                var check = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(warning.WarningMasterId);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }
                var isExists = await _unitOfWork.WarningMasterRepository.GetAllAsync(asd => asd.WarningMasterId != warning.WarningMasterId && asd.WarningName.ToLower().Trim() == warning.WarningName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{warning.WarningName}' already exists" };
                }
                warning.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.WarningMasterRepository.UpdateWarningMaster(warning);
                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(warning.WarningMasterId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record, Please try again later!" };
            }
        }

        [HttpDelete("DeleteWarningMaster")]
        public async Task<APIResponse> DeleteWarningMaster(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var check = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(model.Id);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.WarningMasterRepository.DeleteWarningMaster(model);
                if (result.Id > 0)
                {
                    var deleted = await _unitOfWork.WarningMasterRepository.GetByWarningMasterId(model.Id);
                    return new APIResponse { isSuccess = true, Data = deleted, ResponseMessage = "The record has been deleted successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record, Please try again later!" };
            }
        }
    }
}
