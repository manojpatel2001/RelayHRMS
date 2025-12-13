using HRMS_API.NotificationService.HubService;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveCancellationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public LeaveCancellationAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("GetEmpLeaveCancellationRequestReport")]
        public async Task<APIResponse> GetEmpLeaveCancellationRequestReport([FromBody] LeaveCancellationRequestFilterViewModel model)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetEmpLeaveCancellationRequestReport(model);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }

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
        [HttpPost("GetLeavecancellation")]
        public async Task<APIResponse> GetLeavecancellation([FromBody] LeaveCancellationReportRequest model)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetLeavecancellationReport(model);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }

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

        [HttpPost("CreateLeaveCancellation")]
        public async Task<APIResponse> CreateLeaveCancellation([FromBody] List<LeaveCancellationRequestVM> models)
        {
            try
            {
                if (models == null || !models.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records to save." };

                foreach (var model in models)
                {
                    var spResponse = await _unitOfWork.LeaveCancellationRepository.CreateLeavecancellation(model);
                    if (spResponse.Success == 0)
                    {
                        // If any record fails, return the error message from the stored procedure
                        return new APIResponse { isSuccess = false, ResponseMessage = spResponse.ResponseMessage };
                    }
                }

                // Only return success if all records are saved successfully
                return new APIResponse { isSuccess = true, ResponseMessage = "Leave cancellation saved successfully!" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }
        [HttpPut("UpdateLeaveCancellationStatus")]
        public async Task<APIResponse> UpdateLeaveCancellationStatus(List<updateLeaveCancellationRequestVM> requests)
        {
            try
            {
                if (requests == null || requests.Count == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records selected for update." };
                }

                // Process each request in the list
                foreach (var request in requests)
                {
                    var result = await _unitOfWork.LeaveCancellationRepository.UpdateLeavecancellation(request);

                    if (result.Success <= 0)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = result.ResponseMessage
                        };
                    }
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = $"{requests.Count} leave cancellation request(s) updated successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
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

                var result = await _unitOfWork.LeaveCancellationRepository.DeleteLeavecancellation(DeleteRecord);
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

        [HttpPost("GetReportingWiseLeaveCancellationRequestReport")]
        public async Task<APIResponse> GetReportingWiseLeaveCancellationRequestReport([FromBody] vmLeaveCancellationReportFilter model)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetReportingWiseLeaveCancellationRequestReport(model);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }

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
