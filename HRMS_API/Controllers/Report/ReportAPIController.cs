using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetLeaveBalanceAdmin")]
        public async Task<APIResponse> GetLeaveBalanceAdmin([FromBody] LeaveBalance_ParamForAdmin vm)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetLeaveBalanceForAdmin(vm);

                if (data == null || data.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No leave records found."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Leave records fetched successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve leave records. Please try again later!"
                };
            }
        }


        [HttpGet("ActiveOrInactive")]
        public async Task<APIResponse> ActiveOrInactive(int Compid, string Action)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetActiveOrInactiveUsers(Action, Compid);
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
        [HttpGet("ActiveOrInactiveMobileUsers")]
        public async Task<APIResponse> ActiveOrInactiveMobileUsers(int Compid, string Action)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetActiveOrInactiveMobileUsers(Action, Compid);
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
        [HttpGet("GetHolidaysForYear")]
        public async Task<APIResponse> GetHolidaysForYear(int Year)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetHolidaysForYear(Year);
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

        [HttpPut("UpdateMobileUser")]
        public async Task<APIResponse> UpdateMobileUser([FromBody] UpdateMobileUserStatusRequest model)
        {
            try
            {
                if (model == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }


                var result = await _unitOfWork.ReportRepository.UpdateMobileUsers(model);
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

        [HttpGet("GetYearlySalaryReportForAdmin")]
        public async Task<APIResponse> GetYearlySalaryReportForAdmin(int StartYear, int EnadYear)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetYearlySalaryReportForAdmin(StartYear, EnadYear);
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



        [HttpGet("GetUsedLeavesSummary")]
        public async Task<APIResponse> GetUsedLeavesSummary()
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetUsedLeavesSummary();
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



        [HttpGet("GetEmployeeMonthlyLeaveStatus")]
        public async Task<APIResponse> GetEmployeeMonthlyLeaveStatus(string EmpId, int SelectedMonth, int SelectedYear ,int CompId)
        {
            try
            {
                var (leaveApplications, leaveStatuses) =
                    await _unitOfWork.ReportRepository.GetEmployeeMonthlyLeaveStatus(EmpId, SelectedMonth, SelectedYear, CompId);

                var responseData = new
                {
                    LeaveApplications = leaveApplications,
                    LeaveStatuses = leaveStatuses
                };

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = responseData,
                    ResponseMessage = "Records fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records. Please try again later!"
                };
            }
        }

        [HttpGet("GetEmployeeYearlyLeaveStatus")]
        public async Task<APIResponse> GetEmployeeYearlyLeaveStatus(string EmpId, int CompId, int Year)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetEmployeeYearlyLeaveStatus(EmpId, CompId, Year);
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
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }

            [HttpGet("GetCompoffLapseReminder")]
        public async Task<APIResponse> GetCompoffLapseReminder(DateTime SelectedDate, int LapseDays)
        {
            try
            {
                var data = await _unitOfWork.ReportRepository.GetCompoffLapseReminder(SelectedDate, LapseDays);
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


    }
}
