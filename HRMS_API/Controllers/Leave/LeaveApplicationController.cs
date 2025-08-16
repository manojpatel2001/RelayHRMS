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

                var isexist = await _unitOfWork.LeaveApplicationRepository.GetAsync(asp => asp.EmplooyeId == Leave.EmplooyeId && asp.FromDate == Leave.FromDate && asp.Todate == Leave.Todate);
             
                if (isexist != null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Alredy leave applyed for this period" };


                }
                var isexistfromdate = await _unitOfWork.LeaveApplicationRepository.GetAsync(asp => asp.EmplooyeId == Leave.EmplooyeId && asp.FromDate == Leave.FromDate && asp.Todate != Leave.Todate);

                if (isexistfromdate != null)
                {
                    var issoftdelete = await _unitOfWork.LeaveApplicationRepository.softdelete(Leave);

                }


                var isSaved = await _unitOfWork.LeaveApplicationRepository.InsertLeaveApplicationAsync(Leave);

                if (!isSaved)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to insert leave details." };

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



        [HttpPost("GetLeaveApplicationsforApprove")]
        public async Task<APIResponse> GetLeaveApplicationsforApprove([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationsforApprove(filter);
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


        [HttpPost("LeaveapplicationApproveorReject")]
        public async Task<APIResponse> LeaveapplicationApproveorReject(LeaveaprovalVM LVM)
        {
            try
            {
                LVM.Date = DateTime.Now;

                if (LVM.Ids == null || !LVM.Ids.Any() || string.IsNullOrEmpty(LVM.Status))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid input." };
                }
                

                var isSaved = await _unitOfWork.LeaveApplicationRepository.Updateapproval(LVM.Ids, LVM.Status,LVM.Date);

                if (!isSaved)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to update Comp Off details." };

                if (LVM.Status == "Approved")
                {
                    var leavemanage = await _unitOfWork.CompOffDetailsRepository.UpdateLeavedetails(LVM.Ids, LVM.Status);
                    if (!leavemanage)
                        return new APIResponse
                        { isSuccess = false, ResponseMessage = "Failed to update leave details." };

                }


                return new APIResponse { isSuccess = true, ResponseMessage = "Records updated successfully." };
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


        [HttpPost("GetLeaveApplicationsforApproveAdmin")]
        public async Task<APIResponse> GetLeaveApplicationsforApproveAdmin([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationsforApproveAdmin(filter);
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


        [HttpPost("GetLeaveApproval")]
        public async Task<APIResponse> GetLeaveApproval([FromBody] LeaveApp_Param vm)
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetLeaveApproval(vm);

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


        [HttpPost("GetLeaveType")]
        public async Task<APIResponse> GetLeaveType([FromBody] LeaveDetailsvm vm)
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetLeaveDetails(vm);

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
    }
}
