using HRMS_Core.VM.Leave;
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

    }
}
