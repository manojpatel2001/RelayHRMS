using HRMS_API.NotificationService.HubService;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ExitApplication;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.ExitApplicationAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExitApplicationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        public ExitApplicationAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        [HttpPost("CreateExitAppication")]
        public async Task<APIResponse> CreateExitAppication([FromBody] ExitApplicationVm model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Exit Application details cannot be null." };

                var result = await _unitOfWork.ExitApplicationRepository.CreateExitApplication(model);
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
        [HttpPut("UpdateExitApplication")]
        public async Task<APIResponse> UpdateExitApplication([FromBody] ExitApplicationVm employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }


                var result = await _unitOfWork.ExitApplicationRepository.UpdateExitApplication(employee);
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

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete([FromBody] DeleteRecordVModel deleteRecord)
        {
            try
            {   
                if (deleteRecord == null || deleteRecord.Id == null || !deleteRecord.Id.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No records selected for deletion."
                    };
                }
            
                var result = await _unitOfWork.ExitApplicationRepository.DeleteExitApplication(deleteRecord);
                if (result.Success != 1)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = result.ResponseMessage
                    };
                }
              
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = deleteRecord,
                    ResponseMessage = !string.IsNullOrEmpty(result.ResponseMessage)
                        ? result.ResponseMessage
                        : "Selected record(s) deleted successfully."
                };
            }
            catch (Exception err)
            {
       
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while processing your request. Please try again later."
                };
            }
        }


        [HttpGet("GetExitApplicationById")]
        public async Task<APIResponse> GetExitApplicationById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.ExitApplicationRepository.GetExitApplicationById(EmployeeId);
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

        [HttpPost("GetExitApproval")]
        public async Task<APIResponse> GetExitApproval(ExitApprovalParam model)

        {
            try
            {
                var data = await _unitOfWork.ExitApplicationRepository.GetExitApproval(model);
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
        [HttpPut("UpdateExitApproval")]
        public async Task<APIResponse> UpdateExitApproval(ExitApplicationUpdateparam model)
        {
            try
            {
                var data = await _unitOfWork.ExitApplicationRepository.UpdateExitApproval(model);
                if (data.Success == 1)
                {
                    return new APIResponse()
                    {
                        isSuccess = true,
                        Data = data,
                        ResponseMessage = data.ResponseMessage
                    };
                }
                else if (data.Success == 0)
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        Data = data,
                        ResponseMessage = data.ResponseMessage
                    };
                }
                else 
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        Data = null,
                        ResponseMessage = data.ResponseMessage
                    };
                }
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error occurred: {err.Message}"
                };
            }
        }

    }
}
