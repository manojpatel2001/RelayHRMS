using HRMS_Core.Loan;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoanApplicationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetLoanNamesForDropdown")]
        public async Task<APIResponse> GetLoanNamesForDropdown()
        {
            try
            {
                var data = await _unitOfWork.LoanApplicationRepository.GetLoanNamesForDropdown();
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
        [HttpGet("GetLoanApplication")]
        public async Task<APIResponse> GetLoanApplication(int CompanyId)
        
        {
            try
            {
                var data = await _unitOfWork.LoanApplicationRepository.GetLoanApplication(CompanyId);
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
        [HttpGet("GetLoanDetailsById")]
        public async Task<APIResponse> GetLoanDetailsById(int LoanId)
        {
            try
            {
                var data = await _unitOfWork.LoanApplicationRepository.GetLoanDetailsById(LoanId);
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

        [HttpPost("CreateLoanApplication")]
        public async Task<APIResponse> CreateLoanApplication([FromBody] LoanApplicationViewModel model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Loan details cannot be null." };

                var result = await _unitOfWork.LoanApplicationRepository.CreateLoanApplication(model);
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
        [HttpPut("UpdateLoanApplication")]
        public async Task<APIResponse> UpdateLoanApplication([FromBody] LoanApplicationViewModel employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Loan details cannot be null" };
                }


                var result = await _unitOfWork.LoanApplicationRepository.UpdateLoanApplication(employee);
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
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var result = await _unitOfWork.LoanApplicationRepository.DeleteLoanApplication(DeleteRecord);
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
    }
}
