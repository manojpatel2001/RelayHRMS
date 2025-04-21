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
    public class CityCategoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityCategoryAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllCityCategory")]
        public async Task<APIResponse> GetAllCityCategory()
        {
            try
            {
                var data = await _unitOfWork.CityCategoryRepository.GetAllCityCategory();
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

        [HttpGet("GetByCityCategoryId/{id}")]
        public async Task<APIResponse> GetByCityCategoryId(int id)
        {
            try
            {
                var data = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(id);
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

        [HttpPost("CreateCityCategory")]
        public async Task<APIResponse> CreateCityCategory(CityCategory category)
        {
            try
            {
                if (category == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "City category details cannot be null" };
                }

                if (category.CityCategoryId == 0)
                {
                    var isExists = await _unitOfWork.CityCategoryRepository.GetAllAsync(asd =>  asd.CityCategoryName.ToLower().Trim() == category.CityCategoryName.ToLower().Trim()
                   && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{category.CityCategoryName}' already exists" };
                    }
                    category.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CityCategoryRepository.CreateCityCategory(category);
                    if (result.Id > 0)
                    {
                        var newData = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "The record has been saved successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record" };
                }
                else
                {
                    var check = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(category.CityCategoryId);
                    if (check == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                    }
                    var isExists = await _unitOfWork.CityCategoryRepository.GetAllAsync(asd => asd.CityCategoryId != category.CityCategoryId && asd.CityCategoryName.ToLower().Trim() == category.CityCategoryName.ToLower().Trim()
                    && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{category.CityCategoryName}' already exists" };
                    }
                    category.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CityCategoryRepository.UpdateCityCategory(category);
                    if (result.Id > 0)
                    {
                        var updatedData = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(category.CityCategoryId);
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

        [HttpPut("UpdateCityCategory")]
        public async Task<APIResponse> UpdateCityCategory(CityCategory category)
        {
            try
            {
                if (category == null || category.CityCategoryId == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "City category details cannot be null" };
                }

                var check = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(category.CityCategoryId);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }
                var isExists = await _unitOfWork.CityCategoryRepository.GetAllAsync(asd => asd.CityCategoryId != category.CityCategoryId && asd.CityCategoryName.ToLower().Trim() == category.CityCategoryName.ToLower().Trim()
                   && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{category.CityCategoryName}' already exists" };
                }
                category.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CityCategoryRepository.UpdateCityCategory(category);
                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(category.CityCategoryId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record, Please try again later!" };
            }
        }

        [HttpDelete("DeleteCityCategory")]
        public async Task<APIResponse> DeleteCityCategory(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var check = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(model.Id);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CityCategoryRepository.DeleteCityCategory(model);
                if (result.Id > 0)
                {
                    var deleted = await _unitOfWork.CityCategoryRepository.GetByCityCategoryId(model.Id);
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
