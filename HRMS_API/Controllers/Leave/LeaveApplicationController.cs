using HRMS_Core.Leave;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApplicationController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public LeaveApplicationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddLeaveapplication")]

        public async Task<APIResponse> AddLeaveapplication([FromBody]LeaveApplication Leave)
        {
            try
            {
                Leave.CreatedDate = DateTime.Now;
                Leave.LeaveStatus = "Pending";
                var isSaved = await _unitOfWork.LeaveApplicationRepository.InsertLeaveApplicationAsync(Leave);

                if (!isSaved)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to insert Comp Off details." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Records Added successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add records. Please try again later." };
            }


        }
        [HttpPost("GetReportingperson")]
        public async Task<APIResponse> GetReportingperson(int Empid)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeManageRepository.GetAsync(e => e.Id == Empid);
                if (employee == null || employee.ReportingManagerId == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Employee or Reporting Manager not found."
                    };
                }

                var manager = await _unitOfWork.EmployeeManageRepository.GetAsync(m => m.Id == employee.ReportingManagerId);
                if (manager == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Reporting Manager not found."
                    };
                }

                var result = new
                {
                    Id = manager.Id,
                    FullName = manager.FullName
                };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Reporting manager fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Error occurred while fetching reporting person."
                };
            }
        }


        [HttpPost("GetLeaveApplications")]
        public async Task<APIResponse> GetLeaveApplications([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationsAsync(filter);
                if (result == null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Data Fetched not Sucessfully"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Data Fetched Sucessfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Data not fetched successfully."
                };
            }
        }


    }
}
