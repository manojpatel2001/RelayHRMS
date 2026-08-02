using HRMS_API.Services;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeConversionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmployeeConversionService _employeeConversionService;

        public EmployeeConversionAPIController(IUnitOfWork unitOfWork, EmployeeConversionService employeeConversionService)
        {
            _unitOfWork = unitOfWork;
            _employeeConversionService = employeeConversionService;
        }

        [HttpGet("GetByCandidateApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.CandidateEmployeeConversionRepository.GetByCandidateApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve conversion status. Please try again later." }; }
        }

        // Converts an accepted Candidate into a real Employee Master record — see
        // HRMS_API\Services\EmployeeConversionService.cs for the full flow.
        [HttpPost("ConvertToEmployee")]
        public async Task<APIResponse> ConvertToEmployee([FromBody] ConvertToEmployeeRequest model)
        {
            try
            {
                var (success, message, employeeCode) = await _employeeConversionService.ConvertCandidateToEmployeeAsync(model.CandidateApplicationId, model.ActionBy);
                return new APIResponse { isSuccess = success, ResponseMessage = message, Data = employeeCode };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to convert candidate to employee. Please try again later.", Data = ex.Message };
            }
        }
    }

    public class ConvertToEmployeeRequest
    {
        public int CandidateApplicationId { get; set; }
        public int ActionBy { get; set; }
    }
}
