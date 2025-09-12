using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlySalaryDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MonthlySalaryDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetBySalaryDetailsId/{id}")]
        public async Task<APIResponse> GetBySalaryDetailsId(List<int> id)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetBySalaryDetailsId(id);
                if (data == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record, Please try again later!" };
            }
        }

        [HttpPost("GetAll")]
        public async Task<APIResponse> GetAll(SalaryDetailsParameterVm vm)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetSalaryDetails(vm);
                if(data==null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "Record fetched successfully"
                    };
                }

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }
        [HttpPost("GetSalarySlip")]
        public async Task<APIResponse> GetSalarySlip(salaryslipParamReport vm)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetSalarySlipReport(vm);
                if (data == null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "Record fetched successfully"
                    };
                }

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        } 


        [HttpPost("GetSalarySlipReport")]
        public async Task<APIResponse> GetSalarySlipReport(salaryslipParamReport vm)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetSalarySlipReport(vm);
                if (data == null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "Record fetched successfully"
                    };
                }

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }

        [HttpPost("GetMonthlySalary")]
        public async Task<APIResponse> GetMonthlySalary(MonthlySalaryRequestViewModel vm)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetMonthlySalaryData(vm);
                if (data == null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "Record fetched successfully"
                    };
                }

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }


        [HttpGet("GetYearlySalaryCard")]
        public async Task<APIResponse> GetYearlySalaryCard(int year, int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetYearlySalaryCard(year, EmployeeId);
                if (data == null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "No data found"
                    };
                }
                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }


        [HttpGet("GetEmployeeSalaryDays")]
        public async Task<APIResponse> GetEmployeeSalaryDays( int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetEmployeeSalaryDays(EmployeeId);
                if (data == null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "No data found"
                    };
                }
                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }
        [HttpGet("GetYearlySalaryReport")]
        public async Task<APIResponse> GetYearlySalaryReport(int year, int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetYearlySalarySummaryReport(year, EmployeeId);
                if (data == null || !data.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "No data found"
                    };
                }
                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }



        [HttpPost("CreateMonthlySalary")]
        public async Task<APIResponse> CreateMonthlySalary(MonthlySalaryRequestViewModel vm)
        {
            try
            {
                var result = await _unitOfWork.MonthlySalaryDetailsRepository.CreateSalaryDetails(vm);

                // Check result
                if (result.Success > 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        Data = result,
                        ResponseMessage = "Salary record created successfully."
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = result.ResponseMessage
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Unable to create salary record. Error: {ex.Message}"
                };
            }
        }

        [HttpDelete("DeleteSalaryDetails")]
        public async Task<APIResponse> DeleteSalaryDetails(DeleteRecordVModel model)
        {
            try
            {
                if (model == null )
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }


                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.MonthlySalaryDetailsRepository.DeleteSalaryDetails(model);
           
                    return new APIResponse { isSuccess = true, Data = result, ResponseMessage = "The record has been deleted successfully" };
     

            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record, Please try again later!" };
            }
        }
    }
}
