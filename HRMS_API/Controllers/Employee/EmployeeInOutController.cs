using HRMS_Core.Employee;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<APIResponse> CreateEmpINOut([FromBody] EmployeeInOutRecord model)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                model.Ip_adrress = ipAddress;

                var isexist = await _unitOfWork.EmployeeInOut.GetAsync(asp => asp.Emp_Id == model.Emp_Id && asp.In_Time != null && asp.Out_Time == null);
                if(isexist != null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        
                        ResponseMessage = "You are already in."
                    };
                }
               
                var data = await _unitOfWork.EmployeeInOut.CreateEmpInOut(model);
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
                bool isUpdated = await _unitOfWork.EmployeeInOut.UpdateEmployeeOutTimeAsync(
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
                var data = await _unitOfWork.EmployeeInOut.GetInOutRecord(empid, Month, Year
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
                var data = await _unitOfWork.EmployeeInOut.GetMultipleInOutRecordAsync(empid, Month, Year);


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
                var data = await _unitOfWork.EmployeeInOut.AttendanceMultipleInOutReport(empid, Month, Year);


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
                var data = await _unitOfWork.EmployeeInOut.AttendancefirstInOutReport(empid, Month, Year);


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

                // Step 1: Get report data
                var getdata = await _unitOfWork.EmployeeInOut.GetEmployeeInOutReport(outFilterVM);

                if (getdata == null || !getdata.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                // Step 2: Get regularization data
                var regData = await _unitOfWork.AttendanceRegularizationRepository
                    .GetAllAsync(x => x.IsEnabled== true && x.IsDeleted==false);

                // Step 3: Filter records
                var data = getdata.Where(d =>
                {
                    if (d.Status == "Absent")
                    {
                        // Check if any pending/rejected regularization exists
                        var hasPendingOrRejected = regData.Any(r =>
                            r.EmpId == d.Id &&
                            r.ForDate == d.For_Date &&
                            (r.IsPending || r.IsRejected)
                        );

                        // If pending or rejected record exists, exclude it
                        return !hasPendingOrRejected;
                    }

                    // If not absent, include
                    return true;
                }).ToList();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data fetched successfully."
                };
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

    }
}
