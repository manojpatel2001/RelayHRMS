using HRMS_API.Services;
using HRMS_Core.Probations;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Probations
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbationEvaluationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;
        private readonly EmailService _emailService;

        public ProbationEvaluationAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _emailService = emailService;
        }

        [HttpPost("CreateProbationEvaluationForm")]
        public async Task<APIResponse> CreateProbationEvaluationForm([FromBody] ProbationEvaluationForm model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Probation evaluation form details cannot be null." };

                var result = await _unitOfWork.ProbationEvaluationFormRepository.CreateProbationEvaluationForm(model);
                if (result.Success > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateProbationEvaluationForm")]
        public async Task<APIResponse> UpdateProbationEvaluationForm([FromBody] ProbationEvaluationForm model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Probation evaluation form details cannot be null." };

                var result = await _unitOfWork.ProbationEvaluationFormRepository.UpdateProbationEvaluationForm(model);
                if (result.Success > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteProbationEvaluationForm")]
        public async Task<APIResponse> DeleteProbationEvaluationForm([FromQuery] DeleteRecordVM deleteRecord)
        {
            try
            {
                if (deleteRecord == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.ProbationEvaluationFormRepository.DeleteProbationEvaluationForm(deleteRecord);
                if (result.Success > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        //[HttpGet("GetProbationEvaluationForm")]
        //public async Task<APIResponse> GetProbationEvaluationForm([FromQuery] int? probationEvaluationFormId = null, [FromQuery] int? employeeId = null)
        //{
        //    try
        //    {
        //        var result = await _unitOfWork.ProbationEvaluationFormRepository.GetProbationEvaluationForm(probationEvaluationFormId, employeeId);
        //        return new APIResponse { isSuccess = true, Data = result, ResponseMessage = "Record fetched successfully." };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to fetch record. Please try again later." };
        //    }
        //} 

        [HttpGet("GetProbationEvaluationFormList")]
        public async Task<APIResponse> GetProbationEvaluationFormList([FromQuery] int loggedInUserId)
        {
            try
            {
                if (loggedInUserId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid user id." };

                var result = await _unitOfWork.ProbationEvaluationFormRepository
                                    .GetProbationEvaluationFormList(loggedInUserId);

                return new APIResponse { isSuccess = true, Data = result, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to fetch record. Please try again later." };
            }
        }

    }
}
