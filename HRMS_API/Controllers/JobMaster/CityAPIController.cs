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
    public class CityAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllCity")]
        public async Task<APIResponse> GetAllCity()
        {
            try
            {
                var data = await _unitOfWork.CityRepository.GetAllCity();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetByCityId/{id}")]
        public async Task<APIResponse> GetByCityId(int id)
        {
            try
            {
                var data = await _unitOfWork.CityRepository.GetByCityId(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateCity")]
        public async Task<APIResponse> CreateCity(City model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "City details cannot be null." };

                if (model.CityID == 0)
                {
                    var exists = await _unitOfWork.CityRepository.GetAllAsync(c =>
                        c.StateId == model.StateId &&
                        c.CityName.ToLower().Trim() == model.CityName.ToLower().Trim() &&
                        c.IsDeleted == false);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CityName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CityRepository.CreateCity(model);

                    if (result.Id > 0)
                    {
                        var newCity = await _unitOfWork.CityRepository.GetByCityId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newCity, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var exists = await _unitOfWork.CityRepository.GetAllAsync(c =>
                        c.StateId == model.StateId &&
                        c.CityID != model.CityID &&
                        c.CityName.ToLower().Trim() == model.CityName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled==true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CityName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CityRepository.UpdateCity(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.CityRepository.GetByCityId(model.CityID);
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

        [HttpPut("UpdateCity")]
        public async Task<APIResponse> UpdateCity(City city)
        {
            try
            {
                if (city == null || city.CityID == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "City details cannot be null." };

                var check = await _unitOfWork.CityRepository.GetByCityId(city.CityID);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid city record." };

                var isExists = await _unitOfWork.CityRepository.GetAllAsync(x =>
                    x.StateId == city.StateId &&
                    x.CityID != city.CityID &&
                    x.CityName.ToLower().Trim() == city.CityName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name  '{city.CityName}' already exists." };

                city.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CityRepository.UpdateCity(city);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.CityRepository.GetByCityId(city.CityID);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update city. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record Please try again later." };
            }
        }

        [HttpDelete("DeleteCity")]
        public async Task<APIResponse> DeleteCity(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CityRepository.DeleteCity(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete city. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}

