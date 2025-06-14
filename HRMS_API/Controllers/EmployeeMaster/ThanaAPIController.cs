using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanaAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThanaAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllThanas")]
        public async Task<APIResponse> GetAllThanas()
        {
            try
            {
                var data = await _unitOfWork.ThanaRepository.GetAllThanas();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetThanaById/{id}")]
        public async Task<APIResponse> GetThanaById(int id)
        {
            try
            {
                var data = await _unitOfWork.ThanaRepository.GetThanaById(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateThana")]
        public async Task<APIResponse> CreateThana(Thana model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Thana details cannot be null." };

                if (model.ThanaId == 0)
                {
                    var exists = await _unitOfWork.ThanaRepository.GetAllAsync(t =>
                        t.ThanaName.ToLower().Trim() == model.ThanaName.ToLower().Trim() &&
                        t.IsDeleted == false && t.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.ThanaName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.ThanaRepository.CreateThana(model);

                    if (result.Id > 0)
                    {
                        var newThana = await _unitOfWork.ThanaRepository.GetThanaById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newThana, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var checkCountry = await _unitOfWork.CountryRepository.GetCountryById(model.ThanaId);
                    if (checkCountry == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Please select valid record" };

                    }
                    var exists = await _unitOfWork.ThanaRepository.GetAllAsync(t =>
                        t.ThanaId != model.ThanaId &&
                        t.ThanaName.ToLower().Trim() == model.ThanaName.ToLower().Trim() &&
                        t.IsDeleted == false && t.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.ThanaName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.ThanaRepository.UpdateThana(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.ThanaRepository.GetThanaById((int)result.Id);
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

        [HttpPut("UpdateThana")]
        public async Task<APIResponse> UpdateThana(Thana thana)
        {
            try
            {
                if (thana == null || thana.ThanaId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Thana details cannot be null." };

                var check = await _unitOfWork.ThanaRepository.GetThanaById(thana.ThanaId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid thana record." };

                var isExists = await _unitOfWork.ThanaRepository.GetAllAsync(x =>
                    x.ThanaId != thana.ThanaId &&
                    x.ThanaName.ToLower().Trim() == thana.ThanaName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{thana.ThanaName}' already exists." };

                thana.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ThanaRepository.UpdateThana(thana);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.ThanaRepository.GetThanaById(thana.ThanaId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update thana. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteThana")]
        public async Task<APIResponse> DeleteThana(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var check = await _unitOfWork.ThanaRepository.GetThanaById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid thana record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ThanaRepository.DeleteThana(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete thana. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
