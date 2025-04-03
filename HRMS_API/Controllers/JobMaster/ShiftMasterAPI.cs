using HRMS_Core.Master.JobMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftMasterAPI : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftMasterAPI(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllShifts")]
        public async Task<APIResponse> GetAllShifts()
        {
            try
            {
                var data = await _unitOfWork.ShiftMasterRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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

        [HttpPost("CreateShift")]
        public async Task<APIResponse> CreateShift(ShiftMaster shiftDetails)
        {
            try
            {
                if (shiftDetails == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Shift details cannot be null" };
                }

                
                var isExists = await _unitOfWork.ShiftMasterRepository.GetAllAsync(asd => asd.ShiftName.ToLower().Trim() == shiftDetails.ShiftName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{shiftDetails.ShiftName}' already exists" };
                }
                shiftDetails.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.ShiftMasterRepository.AddAsync(shiftDetails);
                await _unitOfWork.CommitAsync();



                return new APIResponse() { isSuccess = true, Data = shiftDetails, ResponseMessage = "The record has been saved successfully" };

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

    }
}
