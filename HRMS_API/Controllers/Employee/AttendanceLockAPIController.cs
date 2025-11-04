using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceLockAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceLockAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("GetAttendanceLockDetails")]
        public async Task<APIResponse> GetAttendanceLockDetails([FromBody] AttendanceLockParamVm model)
        {
            try
            {
                if (model == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }


                var data = await _unitOfWork.AttendanceLockRepository.GetEmployeeLockStatus(model);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }


    }
}
