using HRMS_Core.Leave;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveOpeningAPIController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public LeaveOpeningAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllLeaveOpening")]
        public async Task<APIResponse> GetAllLeaveOpening()
        {
            try
            {
                var data = await _unitOfWork.LeaveOpeningRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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



        [HttpPost("CreateLeaveOpening")]
        public async Task<APIResponse> CreateLeaveOpening(LeaveOpening leaveOpening)
        {
            try
            {
                if (leaveOpening == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Shift details cannot be null" };
                }

                leaveOpening.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.LeaveOpeningRepository.AddAsync(leaveOpening);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = leaveOpening, ResponseMessage = "The record has been saved successfully" };

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

        [HttpPut("UpdateLeaveOpening")]
        public async Task<APIResponse> UpdateLeaveOpening(LeaveOpening leaveOpening)
        {
            try
            {
                if (leaveOpening == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Invalid earning details provided."
                    };
                }

                await _unitOfWork.LeaveOpeningRepository.UpdateleaveOpening(leaveOpening);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = leaveOpening,
                    ResponseMessage = "The record has been updated successfully."
                };
            }
            catch (Exception err)
            {
                // Optionally log the exception here

                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update the record, please try again later."
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

                var data = await _unitOfWork.LeaveOpeningRepository.SoftDelete(DeleteRecord);
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
