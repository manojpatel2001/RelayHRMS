using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCertificationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeCertificationAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeCertification")]
        public async Task<APIResponse> CreateEmployeeCertification([FromBody] EmployeeCertification model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Certification details cannot be null." };

                if (string.IsNullOrWhiteSpace(model.CertificationName))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Certification Name is required." };

                var result = await _unitOfWork.EmployeeCertificationRepository.CreateEmployeeCertification(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeCertification")]
        public async Task<APIResponse> UpdateEmployeeCertification([FromBody] EmployeeCertification model)
        {
            try
            {
                if (model == null || model.EmployeeCertificationId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid certification record." };

                var result = await _unitOfWork.EmployeeCertificationRepository.UpdateEmployeeCertification(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeCertification")]
        public async Task<APIResponse> DeleteEmployeeCertification([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeCertificationRepository.DeleteEmployeeCertification(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeCertificationByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeCertificationByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeCertificationRepository.GetEmployeeCertificationByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
