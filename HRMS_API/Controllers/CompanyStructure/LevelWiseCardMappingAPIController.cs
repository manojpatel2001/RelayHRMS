using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.CompanyStructure
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelWiseCardMappingAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelWiseCardMappingAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllLevelWiseCardMapping")]
        public async Task<APIResponse> GetAllLevelWiseCardMapping()
        {
            try
            {
                var data = await _unitOfWork.LevelWiseCardMappingRepository.GetAllLevelWiseCardMapping();
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

        [HttpGet("GetByLevelWiseCardMappingId/{id}")]
        public async Task<APIResponse> GetByLevelWiseCardMappingId(int id)
        {
            try
            {
                var data = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(id);
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

        [HttpPost("CreateOrUpdateLevelWiseCardMapping")]
        public async Task<APIResponse> CreateOrUpdateLevelWiseCardMapping(LevelWiseCardMapping model)
        {
            try
            {
                if (model == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Details cannot be null" };
                }

                if (model.LevelWiseCardMappingId == 0)
                {
                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.LevelWiseCardMappingRepository.CreateLevelWiseCardMapping(model);
                    if (result.Id > 0)
                    {
                        var newData = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "The record has been saved successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record" };
                }
                else
                {
                    var check = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(model.LevelWiseCardMappingId);
                    if (check == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                    }

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.LevelWiseCardMappingRepository.UpdateLevelWiseCardMapping(model);
                    if (result.Id > 0)
                    {
                        var updatedData = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(model.LevelWiseCardMappingId);
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

        [HttpPut("UpdateLevelWiseCardMapping")]
        public async Task<APIResponse> UpdateLevelWiseCardMapping(LevelWiseCardMapping model)
        {
            try
            {
                if (model == null || model.LevelWiseCardMappingId == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Details cannot be null" };
                }

                var check = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(model.LevelWiseCardMappingId);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.LevelWiseCardMappingRepository.UpdateLevelWiseCardMapping(model);
                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(model.LevelWiseCardMappingId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record, Please try again later!" };
            }
        }

        [HttpDelete("DeleteLevelWiseCardMapping")]
        public async Task<APIResponse> DeleteLevelWiseCardMapping(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var check = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(model.Id);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.LevelWiseCardMappingRepository.DeleteLevelWiseCardMapping(model);
                if (result.Id > 0)
                {
                    var deletedData = await _unitOfWork.LevelWiseCardMappingRepository.GetByLevelWiseCardMappingId(model.Id);
                    return new APIResponse { isSuccess = true, Data = deletedData, ResponseMessage = "The record has been deleted successfully" };
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
