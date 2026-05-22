using HRMS_API.NotificationService.HubService;
using HRMS_Core.VM.ExitApplication;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.ExitApplicationAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class NOCAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        public NOCAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }


        [HttpPost("CreateNOC")]
        public async Task<APIResponse> CreateNOC([FromBody] NOSForm model)
        {
            try
            {
                // Add validation logging
                if (model == null)
                {

                    return new APIResponse { isSuccess = false, ResponseMessage = "NOC details cannot be null." };
                }

                var result = await _unitOfWork.NOCRepository.CreateNOC(model);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {

                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to add NOC record. Please try again later."
                };
            }
        }

        [HttpPost("UpdateNOC")]
        public async Task<APIResponse> UpdateNOC([FromBody] NOSForm model)
        {
            try
            {
                if (model == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "NOC details cannot be null" };
                }

                var result = await _unitOfWork.NOCRepository.UpdateNOC(model);
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
                    ResponseMessage = "Unable to update NOC record. Please try again later!"
                };
            }
        }

        [HttpGet("GetNOCByGetEmployeeExitDetails")]
        public async Task<APIResponse> GetNOCByGetEmployeeExitDetails(int EmployeeId)
        {
            try
            {
                if (EmployeeId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid Exit Application ID." };

                var result = await _unitOfWork.NOCRepository.GetNOCByGetEmployeeExitDetails(EmployeeId);

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "NOC data retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        [HttpGet("GetNOCByExitApplicationId")]
        public async Task<APIResponse> GetNOCByExitApplicationId(int exitApplicationId)
        {
            try
            {
                if (exitApplicationId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid Exit Application ID." };

                var result = await _unitOfWork.NOCRepository.GetNOCByExitApplicationId(exitApplicationId);

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "NOC data retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }


    }
}
