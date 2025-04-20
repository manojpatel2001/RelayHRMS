using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReasonAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReasonAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllReasons")]
        public async Task<APIResponse> GetAllReasons()
        {
            try
            {
                var data = await _unitOfWork.ReasonRepository.GetAllReasons();
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

        [HttpGet("GetByReasonId/{id}")]
        public async Task<APIResponse> GetByReasonId(int id)
        {
            try
            {
                var data = await _unitOfWork.ReasonRepository.GetByReasonId(id);
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

        [HttpPost("CreateReason")]
        public async Task<APIResponse> CreateReason(Reason reason)
        {
            try
            {
                if (reason == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Reason details cannot be null" };
                }

                if (reason.ReasonId == 0)
                {
                    var isExists = await _unitOfWork.ReasonRepository.GetAllAsync(asd => asd.ReasonName.ToLower().Trim() == reason.ReasonName.ToLower().Trim() 
                    &&asd.ReasonType== reason .ReasonType&& asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{reason.ReasonName}' already exists" };
                    }
                    reason.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.ReasonRepository.CreateReason(reason);
                    if (result.Id > 0)
                    {
                        var newData = await _unitOfWork.ReasonRepository.GetByReasonId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "The record has been saved successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record" };
                }
                else
                {
                    var check = await _unitOfWork.ReasonRepository.GetByReasonId(reason.ReasonId);
                    if (check == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                    }
                    var isExists = await _unitOfWork.ReasonRepository.GetAllAsync(asd =>asd.ReasonId!=reason.ReasonId && asd.ReasonName.ToLower().Trim() == reason.ReasonName.ToLower().Trim()
                    && asd.ReasonType == reason.ReasonType && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{reason.ReasonName}' already exists" };
                    }
                    reason.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.ReasonRepository.UpdateReason(reason);
                    if (result.Id > 0)
                    {
                        var updatedData = await _unitOfWork.ReasonRepository.GetByReasonId(reason.ReasonId);
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

        [HttpPut("UpdateReason")]
        public async Task<APIResponse> UpdateReason(Reason reason)
        {
            try
            {
                if (reason == null || reason.ReasonId == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Reason details cannot be null" };
                }

                var check = await _unitOfWork.ReasonRepository.GetByReasonId(reason.ReasonId);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }
                var isExists = await _unitOfWork.ReasonRepository.GetAllAsync(asd => asd.ReasonId != reason.ReasonId && asd.ReasonName.ToLower().Trim() == reason.ReasonName.ToLower().Trim()
                    && asd.ReasonType == reason.ReasonType && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{reason.ReasonName}' already exists" };
                }
                reason.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ReasonRepository.UpdateReason(reason);
                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.ReasonRepository.GetByReasonId(reason.ReasonId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record, Please try again later!" };
            }
        }

        [HttpDelete("DeleteReason")]
        public async Task<APIResponse> DeleteReason(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var check = await _unitOfWork.ReasonRepository.GetByReasonId(model.Id);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ReasonRepository.DeleteReason(model);
                if (result.Id > 0)
                {
                    var deleted = await _unitOfWork.ReasonRepository.GetByReasonId(model.Id);
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
