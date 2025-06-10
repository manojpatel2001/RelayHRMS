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
    public class CountryAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public CountryAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllCountries")]
        public async Task<APIResponse> GetAllCountries()
        {
            try
            {
                var data = await _unitOfWork.CountryRepository.GetAllCountries();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetCountryById/{id}")]
        public async Task<APIResponse> GetCountryById(int id)
        {
            try
            {
                var data = await _unitOfWork.CountryRepository.GetCountryById(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateCountry")]
        public async Task<APIResponse> CreateCountry(Country model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Country details cannot be null." };

                if (model.CountryId == 0)
                {
                    var exists = await _unitOfWork.CountryRepository.GetAllAsync(c =>
                        c.CountryName.ToLower().Trim() == model.CountryName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CountryName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CountryRepository.CreateCountry(model);

                    if (result.Id > 0)
                    {
                        var newCountry = await _unitOfWork.CountryRepository.GetCountryById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newCountry, ResponseMessage = "The record has been saved successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var checkCountry = await _unitOfWork.CountryRepository.GetCountryById(model.CountryId);
                    if (checkCountry == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Please select valid record" };

                    }

                    var exists = await _unitOfWork.CountryRepository.GetAllAsync(c =>
                        c.CountryId != model.CountryId &&
                        c.CountryName.ToLower().Trim() == model.CountryName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CountryName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CountryRepository.UpdateCountry(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.CountryRepository.GetCountryById(model.CountryId);
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

        [HttpPut("UpdateCountry")]
        public async Task<APIResponse> UpdateCountry(Country country)
        {
            try
            {
                if (country == null || country.CountryId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Country details cannot be null." };

                var check = await _unitOfWork.CountryRepository.GetCountryById(country.CountryId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var isExists = await _unitOfWork.CountryRepository.GetAllAsync(x =>
                    x.CountryId != country.CountryId &&
                    x.CountryName.ToLower().Trim() == country.CountryName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{country.CountryName}' already exists." };

                country.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CountryRepository.UpdateCountry(country);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.CountryRepository.GetCountryById(country.CountryId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update country. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteCountry")]
        public async Task<APIResponse> DeleteCountry(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.CountryRepository.GetCountryById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CountryRepository.DeleteCountry(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete country. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
