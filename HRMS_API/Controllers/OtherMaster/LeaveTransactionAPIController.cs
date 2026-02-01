using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveTransactionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTransactionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertLeaveTransaction")]
        public async Task<APIResponse> InsertLeaveTransaction(InsertLeaveTransactionRequest leaveData)
        {
            try
            {
                var data = await _unitOfWork.LeaveTransactionRepository.InsertLeaveTransaction(leaveData);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to insert data. Please try again later." };
            }
        }

      

        [HttpGet("GetAllLeaveTypeByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllLeaveTypeByCompanyId(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.LeaveTransactionRepository.GetAllLeaveTypeByCompanyId(CompanyId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpGet("GetLeaveBalanceReport/{CompanyId}")]
        public async Task<APIResponse> GetLeaveBalanceReport(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.LeaveTransactionRepository.GetLeaveBalanceReport(CompanyId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

    }
}
