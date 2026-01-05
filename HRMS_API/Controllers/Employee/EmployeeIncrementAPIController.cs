using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeIncrementAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeIncrementAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllIncrementReason")]
        public async Task<APIResponse> GetAllIncrementReason()
        {
            try
            {
                var data = await _unitOfWork.EmployeeIncrementRespository.GetAllIncrementReason();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("InsertEmployeeSalaryHistory")]
        public async Task<APIResponse> InsertEmployeeSalaryHistory(InsertEmployeeSalaryHistoryVM salaryPara)
        {
            try
            {
                var data = await _unitOfWork.EmployeeIncrementRespository.InsertEmployeeSalaryHistory(salaryPara);

                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add. Please try again later." };
            }
        }

        [HttpGet("GetAllIncrementEmployees/{companyId}")]
        public async Task<APIResponse> GetAllIncrementEmployees(int companyId)
        {
            try
            {
             
                var result = await _unitOfWork.EmployeeIncrementRespository.GetAllIncrementEmployees(companyId);
                return result;
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Failed to fetch increment records"
                };
            }
        }
        [HttpGet("GetEmployeeSalaryInfo/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeSalaryInfo(int EmployeeId)
        {
            try
            {
             
                var result = await _unitOfWork.EmployeeIncrementRespository.GetEmployeeSalaryInfo(EmployeeId);
                return result;
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Failed to fetch increment records"
                };
            }
        }

        [HttpDelete("DeleteIncrement")]
        public async Task<APIResponse> DeleteIncrement(DeleteRecordVM delete )
        {
            try
            {
                var data = await _unitOfWork.EmployeeIncrementRespository.DeleteIncrement(delete.Id);

                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete. Please try again later." };
            }
        }
    }
}
