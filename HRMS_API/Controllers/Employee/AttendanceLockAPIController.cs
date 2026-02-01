using HRMS_Core.Employee;
using HRMS_Core.VM;
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

        [HttpPost("CreateAttendanceLockEmp")]
        public async Task<APIResponse> CreateAttendanceLockEmp([FromBody] List<AttendanceLock> models)
        {
            try
            {
                if (models == null || !models.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "Attendance lock details cannot be empty." };

                var results = new List<SP_Response>();

                // Loop through each employee and lock attendance
                foreach (var model in models)
                {
                    var result = await _unitOfWork.AttendanceLockRepository.CreateAttendanceLockEmp(model);
                    results.Add(result);
                }

                // Count successes and failures
                var successCount = results.Count(r => r.Success > 0);
                var failedCount = results.Count(r => r.Success <= 0);

                if (failedCount == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = $"Successfully locked attendance for {successCount} employee(s)."
                    };
                }
                else if (successCount == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Failed to lock attendance for all employees."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = $"Locked {successCount} out of {models.Count} employees. {failedCount} failed."
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to add records. Please try again later."
                };
            }
        }


        [HttpPut("UpdateAttendanceLock")]
        public async Task<APIResponse> UpdateAttendanceLock([FromBody] List<AttendanceLock> models)
        {
            try
            {
                if (models == null || !models.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "Attendance unlock details cannot be empty." };

                var results = new List<SP_Response>();

                // Loop through each employee and unlock attendance
                foreach (var model in models)
                {
                    var result = await _unitOfWork.AttendanceLockRepository.UpdateAttendanceLockEmp(model);
                    results.Add(result);
                }

                // Count successes and failures
                var successCount = results.Count(r => r.Success > 0);
                var failedCount = results.Count(r => r.Success <= 0);

                if (failedCount == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = $"Successfully unlocked attendance for {successCount} employee(s)."
                    };
                }
                else if (successCount == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Failed to unlock attendance for all employees."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = $"Unlocked {successCount} out of {models.Count} employees. {failedCount} failed."
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to update records. Please try again later."
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

                var result = await _unitOfWork.AttendanceLockRepository.DeleteAttendanceLockEmp(DeleteRecord);
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
