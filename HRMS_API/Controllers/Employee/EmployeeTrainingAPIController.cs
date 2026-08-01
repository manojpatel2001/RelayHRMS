using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTrainingAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeTrainingAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeTraining")]
        public async Task<APIResponse> CreateEmployeeTraining([FromBody] EmployeeTraining model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Training details cannot be null." };

                if (string.IsNullOrWhiteSpace(model.TrainingName))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Training Name is required." };

                var result = await _unitOfWork.EmployeeTrainingRepository.CreateEmployeeTraining(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeTraining")]
        public async Task<APIResponse> UpdateEmployeeTraining([FromBody] EmployeeTraining model)
        {
            try
            {
                if (model == null || model.EmployeeTrainingId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid training record." };

                var result = await _unitOfWork.EmployeeTrainingRepository.UpdateEmployeeTraining(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeTraining")]
        public async Task<APIResponse> DeleteEmployeeTraining([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeTrainingRepository.DeleteEmployeeTraining(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeTrainingByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeTrainingByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeTrainingRepository.GetEmployeeTrainingByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
