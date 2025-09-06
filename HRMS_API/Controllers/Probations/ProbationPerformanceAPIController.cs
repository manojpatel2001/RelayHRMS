using HRMS_API.Services;
using HRMS_Core.Probations;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Probations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProbationPerformanceAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;
        public ProbationPerformanceAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        [HttpGet("GetAllProbationEmployees/{ProbationManagerId}")]
        public async Task<APIResponse> GetAllProbationEmployees(int ProbationManagerId)
        {
            try
            {
                var data = await _unitOfWork.ProbationPerformanceRepository.GetAllProbationEmployees(ProbationManagerId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeForProbationByEmployeeId/{employeeId}")]
        public async Task<APIResponse> GetEmployeeForProbationByEmployeeId(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ProbationPerformanceRepository.GetEmployeeForProbationByEmployeeId(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateProbationPerformance")]
        public async Task<APIResponse> CreateProbationPerformance([FromForm] ProbationPerformance model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Probation performance details cannot be null." };
                if (model.DocumentFile != null)
                {
                    if (model.DocumentFile.Length > 0)
                    {

                        var folder = $"uploads/employee-probation-file";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.DocumentFile, folder, null);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.DocumentUrl = fileUrl;
                    }
                    
                }
                var result = await _unitOfWork.ProbationPerformanceRepository.CreateProbationPerformance(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateProbationPerformance")]
        public async Task<APIResponse> UpdateProbationPerformance(ProbationPerformance probationPerformance)
        {
            try
            {
                if (probationPerformance == null || probationPerformance.ProbationPerformanceId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Probation performance details cannot be null." };

                var result = await _unitOfWork.ProbationPerformanceRepository.UpdateProbationPerformance(probationPerformance);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteProbationPerformance")]
        public async Task<APIResponse> DeleteProbationPerformance(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.ProbationPerformanceRepository.DeleteProbationPerformance(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
