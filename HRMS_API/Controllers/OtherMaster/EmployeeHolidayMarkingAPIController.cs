using DocumentFormat.OpenXml.Wordprocessing;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeHolidayMarkingAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeHolidayMarkingAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEmployeeHoliday")]
        public async Task<APIResponse> CreateEmployeeHoliday([FromBody] EmployeeHolidayMarking model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "LeftEmployee details cannot be null." };

                var result = await _unitOfWork.EmployeeHolidayMarkingRepository.CreateEmployeeHoliday(model);
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

        [HttpPut("UpdateEmployeeHoliday")]
        public async Task<APIResponse> UpdateEmployeeHoliday([FromBody] EmployeeHolidayMarking employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }


                var result = await _unitOfWork.EmployeeHolidayMarkingRepository.UpdateEmployeeHoliday(employee);
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

        [HttpGet("GetAllEmployeeHoliday")]
        public async Task<APIResponse> GetAllEmployeeHoliday()
        {
            try
            {
                var data = await _unitOfWork.EmployeeHolidayMarkingRepository.GetAllEmployeeHoliday();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(int Id ,int DeletedBy)
        {
            try
            {
                if (Id == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var result = await _unitOfWork.EmployeeHolidayMarkingRepository.DeleteEmployeeHoliday(Id ,DeletedBy);
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
                    ResponseMessage = "Unable to delete records, Please try again later!"
                };
            }
        }

    }
}
