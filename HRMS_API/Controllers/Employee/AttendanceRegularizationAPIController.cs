using HRMS_Core.Employee;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceRegularizationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceRegularizationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDeduction")]
        public async Task<APIResponse> GetAllDeduction()
        {
            try
            {
                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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


        [HttpGet("GetById/{id}")]
        public async Task<APIResponse> GetById(int Id)
        {
            try
            {
                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(x => x.AttendanceRegularizationId == Id && x.IsEnabled == true && x.IsDeleted == false);
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

        [HttpPost("CreateAttendanceRegularization")]
        public async Task<APIResponse> CreateAttendanceRegularization(List<AttendanceRegularization> attendances)
        {
            try
            {
                if (attendances == null || attendances.Count == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "No attendance details received." };
                }

                if (_unitOfWork == null || _unitOfWork.AttendanceRegularizationRepository == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Internal server error: repository not initialized." };
                }

                foreach (var attendance in attendances)
                {
                    if (attendance == null)
                    {
                        continue;
                    }

                    attendance.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.AttendanceRegularizationRepository.AddAsync(attendance);
                }

                await _unitOfWork.CommitAsync();

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = attendances,
                    ResponseMessage = "Records have been saved successfully"
                };
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


        [HttpPut("UpdateAttendanceRegularization")]
        public async Task<APIResponse> UpdateAttendanceRegularization(List<AttendanceRegularization> attendances)
        {
            try
            {
                if (attendances == null || !attendances.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No attendance records provided."
                    };
                }

                foreach (var attendance in attendances)
                {
                    var record = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(x => x.AttendanceRegularizationId == attendance.AttendanceRegularizationId && x.IsEnabled == true && x.IsDeleted == false);
                    if (record != null)
                    {                
                        await _unitOfWork.AttendanceRegularizationRepository.UpdateAttendanceRegularization(record);
                    }
                }

                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Status updated successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update the records, please try again later."
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

                var data = await _unitOfWork.AttendanceRegularizationRepository.SoftDelete(DeleteRecord);
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


        [HttpPost("GetAttendanceRegularization")]
        public async Task<APIResponse> GetAttendanceRegularization([FromForm] AttendanceRegularizationSearchFilterVM attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }
                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceRegularization(attendance);


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
