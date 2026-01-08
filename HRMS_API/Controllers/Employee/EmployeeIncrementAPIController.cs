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

        [HttpPost("InsertBulkEmployeeSalaryHistory")]
        public async Task<APIResponse> InsertBulkEmployeeSalaryHistory(List<InsertEmployeeSalaryHistoryVM> salaryPara)
        {
            try
            {
                if (salaryPara == null || salaryPara.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No data received."
                    };
                }

                foreach (var item in salaryPara)
                {
                    if (item.NewBasicSalary < item.OldBasicSalary)
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "New Basic Salary cannot be less than Old Basic Salary"
                        };

                    if (item.NewGrossSalary < item.OldGrossSalary)
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "New Gross Salary cannot be less than Old Gross Salary"
                        };

                    if (item.NewGrossSalary < item.NewBasicSalary)
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "Gross Salary cannot be less than Basic Salary"
                        };

                    if (item.EffectiveFromDate==null)
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "Effective Date is required"
                        };
                    if (item.ReasonId <= 0|| item.ReasonId==null)
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "Reason is required"
                        };
                }

                foreach (var item in salaryPara)
                {
                    var result = await _unitOfWork.EmployeeIncrementRespository.InsertEmployeeSalaryHistory(item);
                }
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Salary increment saved successfully"
                };
            }
            catch (Exception ex)
            {
                // 🔥 LOG ex here
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to add. Please try again later."
                };
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
        [HttpGet("GetEmployeeSalaryInfoByCompnayId/{companyId}")]
        public async Task<APIResponse> GetEmployeeSalaryInfoByCompnayId(int companyId)
        {
            try
            {
             
                var result = await _unitOfWork.EmployeeIncrementRespository.GetEmployeeSalaryInfoByCompnayId(companyId);
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
