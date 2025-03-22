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
                var data = await _unitOfWork.CityRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }


        [HttpGet("GetByCityId/{id}")]
        public async Task<APIResponse> GetByCityId(int CityId)
        {
            try
            {
                var data = await _unitOfWork.CityRepository.GetAsync(x => x.CityID == CityId && x.IsEnabled == true && x.IsDeleted == false);
                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrive records, Please try again later!"
                };
            }

        }


        [HttpPost("CreateCity")]
        public async Task<APIResponse> CreateCity(City city)
        {
            try
            {
                if (city == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "City details cannot be null" };
                }

                var isExists = await _unitOfWork.CityRepository.GetAllAsync(asd => asd.CityName.ToLower().Trim() == city.CityName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{city.CityName}' already exists" };
                }
                city.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.CityRepository.AddAsync(city);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = city, ResponseMessage = "The record has been saved successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, Please try again later!"
                };
            }

        }


        [HttpPut("UpdateCity")]
        public async Task<APIResponse> UpdateCity(City city)
        {
            try
            {
                if (city == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "City details cannot be null" };
                }

                var oldBranch = await _unitOfWork.CityRepository.GetAsync(asd => asd.CityID == city.CityID && asd.IsEnabled == true && asd.IsDeleted == false);

                if (oldBranch != null)
                {
                    bool isDeleted = await _unitOfWork.CityRepository.UpdateCIty(city);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = city, ResponseMessage = "The record has been updated successfully" };
                }
                else
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                }

            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update record, Please try again later!"
                };
            }
        }


        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var data = await _unitOfWork.CityRepository.SoftDelete(DeleteRecord);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = DeleteRecord, ResponseMessage = "The record has been deleted successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to delete records, Please try again later!"
                };
            }
        }
    }
}
