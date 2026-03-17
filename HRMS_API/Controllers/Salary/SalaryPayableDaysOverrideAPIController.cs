using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryPayableDaysOverrideAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public SalaryPayableDaysOverrideAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Create")]
        public async Task<APIResponse> Create([FromBody] SalaryPayableDaysOverrideResponseDto dto)
        {
            try
            {
                if (dto == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Request body cannot be null." };

                var result = await _unitOfWork.SalaryPayableDaysOverrideRepository.CreateSalaryPayableDaysOverride(dto);

                if (result.Success > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        // ──────────────────────────────────────────────
        //  PUT api/SalaryPayableDaysOverride/Update
        // ──────────────────────────────────────────────
        [HttpPut("Update")]
        public async Task<APIResponse> Update([FromBody] SalaryPayableDaysOverrideResponseDto dto)
        {
            try
            {
                if (dto == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Request body cannot be null." };

                var result = await _unitOfWork.SalaryPayableDaysOverrideRepository.UpdateSalaryPayableDaysOverride(dto);

                if (result.Success > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        // ──────────────────────────────────────────────
        //  DELETE api/SalaryPayableDaysOverride/Delete
        // ──────────────────────────────────────────────
        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete([FromBody] DeleteRecordVM deleteRecord)
        {
            try
            {
                if (deleteRecord == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Request body cannot be null." };

                var result = await _unitOfWork.SalaryPayableDaysOverrideRepository.DeleteSalaryPayableDaysOverride(deleteRecord);

                if (result.Success > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
