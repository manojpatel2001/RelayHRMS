using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSalaryAllowanceAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSalaryAllowanceAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetEmployeeSalaryAllowanceByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeSalaryAllowanceByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeSalaryAllowanceRepository.GetEmployeeSalaryAllowanceByEmployeeId(EmployeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpGet("GetLiveEmployeeSalaryAllowance/{GrossSalary}")]
        public async Task<APIResponse> GetLiveEmployeeSalaryAllowance(decimal GrossSalary)
        {
            try
            {
                var data = await _unitOfWork.EmployeeSalaryAllowanceRepository.GetLiveEmployeeSalaryAllowance(GrossSalary);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
    }
}
