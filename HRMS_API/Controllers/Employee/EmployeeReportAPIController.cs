using HRMS_Core.Employee;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeReportAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeReportAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [HttpPost("Attendencereport")]
   
        public async Task<APIResponse> Attendencereport([FromBody] AttendanceReportVm model)
        {
            try
            {
              
                var data = await _unitOfWork.EmployeeReport.GetAttendanceReport(model);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Records have been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to add records. Please try again later."
                };
            }
        }

        [HttpPost("GetAttendanceReportForAdmin")]
   
        public async Task<APIResponse> GetAttendanceReportForAdmin([FromBody] AttendanceReportforAdminVm model)
        {
            try
            {
              
                var data = await _unitOfWork.EmployeeReport.GetAttendanceReportForAdmin(model);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Records have been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to add records. Please try again later."
                };
            }
        }

        [HttpPost("GetAttendanceCalender")]
        public async Task<APIResponse> GetAttendanceCalender(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeReport.GetAttendanceCalender(commonParameter);
                var newData = new
                {
                    AttedanceCalanderDays = data.AttedanceCalanderDays,
                    AttedanceCalanderDaysSummary = data.AttedanceCalanderDaysSummary.FirstOrDefault()
                };
                return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }





        [HttpPost("ShiftReport")]

        public async Task<APIResponse> ShiftReport([FromBody] AttendanceReportVm model)
        {
            try
            {

                var data = await _unitOfWork.EmployeeReport.GetShiftReport(model);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Records have been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to add records. Please try again later."
                };
            }
        }

    }
}
