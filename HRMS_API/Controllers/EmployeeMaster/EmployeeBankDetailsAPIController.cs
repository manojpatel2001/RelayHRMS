using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeBankDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeBankDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEmployeeBankDetails")]
        public async Task<APIResponse> CreateEmployeeBankDetails(EmployeeBankDetailsModel model)
        {
            try
            {
                var data = await _unitOfWork.EmployeeBankDetailsRepository.CreateEmployeeBankDetails(model);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create bank details. Please try again later." };
            }
        }

        [HttpPost("UpdateEmployeeBankDetails")]
        public async Task<APIResponse> UpdateEmployeeBankDetails(EmployeeBankDetailsModel model)
        {
            try
            {
                var data = await _unitOfWork.EmployeeBankDetailsRepository.UpdateEmployeeBankDetails(model);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update bank details. Please try again later." };
            }
        }

        [HttpPost("DeleteEmployeeBankDetails")]
        public async Task<APIResponse> DeleteEmployeeBankDetails(DeleteRecordVM delete)
        {
            try
            {
                var data = await _unitOfWork.EmployeeBankDetailsRepository.DeleteEmployeeBankDetails(delete);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete bank details. Please try again later." };
            }
        }

        [HttpGet("GetAllEmployeeBankDetails")]
        public async Task<APIResponse> GetAllEmployeeBankDetails()
        {
            try
            {
                var data = await _unitOfWork.EmployeeBankDetailsRepository.GetAllEmployeeBankDetails();
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to fetch bank details. Please try again later." };
            }
        }
    }
}

