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


        [HttpPost("GetAll")]
        public async Task<APIResponse> GetAll(SalaryDetailsParameterVm vm)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetSalaryDetails(vm);

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
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.CreateSalaryDetails(vm);

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
    }
}
