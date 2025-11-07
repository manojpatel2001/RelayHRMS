using Dapper;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Ess.InOut;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInOutController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeInOutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEmpINOut")]
        public async Task<APIResponse> CreateEmpINOut([FromBody] vmInOut model)
        {
            try
            {

                var data = await _unitOfWork.EmployeeInOutRepository.CreateEmpInOut(model);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to punch for now. Please try again later." };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Punch have been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to punch for now. Please try again later."
                };
            }
        }

        [HttpPost("UpdateEmpOutTime")]
        public async Task<APIResponse> UpdateEmpOutTime([FromBody] EmployeeInOutRecord request)
        {
            try
            {
                if (request.Emp_Id == null || request.For_Date == null || request.Out_Time == null || string.IsNullOrWhiteSpace(request.UpdatedBy))
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id, For_Date, Out_Time, and UpdatedBy are required."
                    };
                }
                bool isUpdated = await _unitOfWork.EmployeeInOutRepository.UpdateEmployeeOutTimeAsync(
                    request.Emp_Id.Value,
                    request.For_Date.Value,
                    request.Out_Time.Value,
                    request.UpdatedBy
                );

                if (!isUpdated)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "You are already out."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Out time updated successfully."
                };
            }
            catch (Exception ex)
            {
                // Optional: Log ex
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }

        [HttpPost("GetMonthlyAttendanceDetails")]
        public async Task<APIResponse> GetMonthlyAttendanceDetails(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOutRepository.GetMonthlyAttendanceDetails(vmInOutParameter);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpPost("GetMonthlyAttendanceLog")]
        public async Task<APIResponse> GetMonthlyAttendanceLog(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOutRepository.GetMonthlyAttendanceLog(vmInOutParameter);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("GetDateWiseAttendanceDetails")]
        public async Task<APIResponse> GetDateWiseAttendanceDetails(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOutRepository.GetDateWiseAttendanceDetails(vmInOutParameter);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetEmployeesByReportingManager/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeesByReportingManager(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOutRepository.GetEmployeesByReportingManager(EmployeeId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("FirstInOut")]
        public async Task<APIResponse> FirstInOut([FromForm] int empid, [FromForm] string Month, [FromForm] string Year)
        {
            try
            {
                if (empid == null || Month == null || Year == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }
                var data = await _unitOfWork.EmployeeInOutRepository.GetInOutRecord(empid, Month, Year
               );

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
                    ResponseMessage = "Data fetch successfully."
                };
            }
            catch (Exception ex)
            {
                // Optional: Log ex
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }
        [HttpPost("MultipleOut")]
        public async Task<APIResponse> MultipleOut([FromForm] int empid, [FromForm] string Month, [FromForm] string Year)
        {
            try
            {
                if (empid == null || Month == null || Year == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }
                var data = await _unitOfWork.EmployeeInOutRepository.GetMultipleInOutRecordAsync(empid, Month, Year);


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
                // Optional: Log ex
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }


        [HttpPost("AttendanceInOutReport")]
        public async Task<APIResponse> AttendanceInOutReport([FromForm] int empid, [FromForm] string Month, [FromForm] string Year)
        {
            try
            {
                if (empid == null || Month == null || Year == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }
                var data = await _unitOfWork.EmployeeInOutRepository.AttendanceMultipleInOutReport(empid, Month, Year);


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

        [HttpPost("AttendanceFirstInOutReport")]
        public async Task<APIResponse> AttendanceFirstInOutReport([FromForm] int empid, [FromForm] string Month, [FromForm] string Year)
        {
            try
            {
                if (empid == null || Month == null || Year == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }
                var data = await _unitOfWork.EmployeeInOutRepository.AttendancefirstInOutReport(empid, Month, Year);


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

        [HttpPost("GetEmployeeInOutReport")]
        public async Task<APIResponse> GetEmployeeInOutReport([FromForm] EmployeeInOutFilterVM outFilterVM)
        {
            try
            {
                if (outFilterVM == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id, Month, Year are required."
                    };
                }
                var data = await _unitOfWork.EmployeeInOutRepository.GetEmployeeInOutReport(outFilterVM);

                return data;
                //if (data == null || !data.Any())
                //{
                //    return new APIResponse
                //    {
                //        isSuccess = false,
                //        ResponseMessage = "No matching IN record found or update failed."
                //    };
                //}
                //return new APIResponse
                //{
                //    isSuccess = true,
                //    Data = data,
                //    ResponseMessage = "Data fetched successfully."
                //};
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while fetching report."
                };
            }
        }

        [HttpPost("GetEmpInOutReportForAdmin")]
        public async Task<APIResponse> GetEmpInOutReportForAdmin(EmpInOutReportFilter fiter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOutRepository.GetEmpInOutReportForAdmin(fiter);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("GetAttendanceRegularizationAlerts")]
        public async Task<APIResponse> GetAttendanceRegularizationAlerts(CommonParameter model)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOutRepository.GetAttendanceRegularizationAlerts(model);

                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to fetch data. Please try again later." };
            }
        }


    }
}
