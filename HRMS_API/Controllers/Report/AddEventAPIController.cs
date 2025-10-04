using HRMS_Core.Report;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddEventAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddEventAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEvent")]
        public async Task<APIResponse> CreateEvent([FromBody] AddEvent model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "LeftEmployee details cannot be null." };

                var result = await _unitOfWork.AddeventRepository.CreateEvent(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };

            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }
        [HttpPut("UpdateEvent")]
        public async Task<APIResponse> UpdateEvent([FromBody] AddEvent employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }


                var result = await _unitOfWork.AddeventRepository.UpdateEvent(employee);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
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
        public async Task<APIResponse> Delete(DeleteRecordVModel model)
        {
            try
            {
                if (model == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }


                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.AddeventRepository.DeleteEvent(model);

                return new APIResponse { isSuccess = true, Data = result, ResponseMessage = "The record has been deleted successfully" };


            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record, Please try again later!" };
            }
        }

        [HttpGet("GetAllEvent/{TargetDate}")]
        public async Task<APIResponse> GetAllEvent(DateTime TargetDate)
        {
            try
            {
                var data = await _unitOfWork.AddeventRepository.GetAllEvent(TargetDate);
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
        [HttpGet("GetUserEvents/{UserId}")]
        public async Task<APIResponse> GetUserEvents(int UserId)
        {
            try
            {
                var data = await _unitOfWork.AddeventRepository.GetUserEvent(UserId);
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


    }
}
