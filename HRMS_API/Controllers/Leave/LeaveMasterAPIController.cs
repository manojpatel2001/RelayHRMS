using HRMS_Core.Leave;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet("LeaveType")]
        public async Task<APIResponse> LeaveType()
        {
            try
            {
                var data = await _unitOfWork.LeaveMasterRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }
                var newdata = data.Select(leave => new
                {
                    leavtypeid = leave.Leave_TypeId,
                    LeaveName = leave.Leave_Name
                });
                return new APIResponse() { isSuccess = true, Data = newdata, ResponseMessage = "Record fetched successfully" };
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


        [HttpPost("AddLeavemanage")]
        public async Task<APIResponse> AddLeavemanage(List<int> id,string status)
        {
            try
            {
               
                var data = await _unitOfWork.CompOffDetailsRepository.UpdateLeaveMange(id,status);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }
                return new APIResponse
                {
                    isSuccess = false,
                    Data =data,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
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


    }
}
