using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoLocationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GeoLocationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllGeoLocations/{companyId}")]
        public async Task<APIResponse> GetAllGeoLocations(int companyId)
        {
            try
            {
                var data = await _unitOfWork.GeoLocationRepository.GetAllGeoLocations(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        
        [HttpPost("CreateGeoLocation")]
        public async Task<APIResponse> CreateGeoLocation([FromBody] GeoLocation model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "GeoLocation details cannot be null." };
                var result = await _unitOfWork.GeoLocationRepository.CreateGeoLocation(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateGeoLocation")]
        public async Task<APIResponse> UpdateGeoLocation([FromBody] GeoLocation geoLocation)
        {
            try
            {
                if (geoLocation == null || geoLocation.GeoLocationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "GeoLocation details cannot be null." };
                var result = await _unitOfWork.GeoLocationRepository.UpdateGeoLocation(geoLocation);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteGeoLocation")]
        public async Task<APIResponse> DeleteGeoLocation([FromBody] DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var result = await _unitOfWork.GeoLocationRepository.DeleteGeoLocation(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpPost("CreateAssignGeoLocation")]
        public async Task<APIResponse> CreateAssignGeoLocation(AssignGeoLocation model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "GeoLocation details cannot be null." };
                var result = await _unitOfWork.GeoLocationRepository.CreateAssignGeoLocation(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpGet("GetAssignGeoLocationsWithLocation/{companyId}")]
        public async Task<APIResponse> GetAssignGeoLocationsWithLocation(int companyId)
        {
            try
            {
                var data = await _unitOfWork.GeoLocationRepository.GetAssignGeoLocationsWithLocation(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpDelete("DeleteAssignGeoLocation")]
        public async Task<APIResponse> DeleteAssignGeoLocation(AssignGeoLocation model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.EmployeeIds) )
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var result = await _unitOfWork.GeoLocationRepository.DeleteAssignGeoLocation(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpPost("GetAllEmployeeAndLocation")]
        public async Task<APIResponse> GetAllEmployeeAndLocation(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.GeoLocationRepository.GetAllEmployeeAndLocation(commonParameter);
                var newData = new
                {
                    Employees = data.Employees,
                    Locations = data.Locations
                };
                return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

    }
}
