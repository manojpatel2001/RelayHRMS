using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSalaryStructureTemplateAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSalaryStructureTemplateAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AssignTemplateToEmployee")]
        public async Task<APIResponse> AssignTemplateToEmployee([FromBody] EmployeeSalaryStructureTemplate model)
        {
            try
            {
                if (model == null || model.EmployeeId == 0 || model.SalaryStructureTemplateId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee and template are required." };
                return await _unitOfWork.EmployeeSalaryStructureTemplateRepository.AssignTemplateToEmployee(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to assign template. Please try again later." }; }
        }

        [HttpPost("UnassignEmployeeTemplate")]
        public async Task<APIResponse> UnassignEmployeeTemplate(int employeeId, int? updatedBy)
        {
            try { return await _unitOfWork.EmployeeSalaryStructureTemplateRepository.UnassignEmployeeTemplate(employeeId, updatedBy); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to revert to the grade default. Please try again later." }; }
        }

        [HttpGet("GetEmployeeTemplate/{employeeId}")]
        public async Task<APIResponse> GetEmployeeTemplate(int employeeId)
        {
            try { return await _unitOfWork.EmployeeSalaryStructureTemplateRepository.GetEmployeeTemplate(employeeId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve the employee's template. Please try again later." }; }
        }

        [HttpGet("GetEffectiveTemplate")]
        public async Task<APIResponse> GetEffectiveTemplate(int employeeId, int companyId, int? gradeId, int? designationId)
        {
            try
            {
                var result = await _unitOfWork.EmployeeSalaryStructureTemplateRepository.GetEffectiveTemplate(employeeId, companyId, gradeId, designationId);
                if (result == null || result.SalaryStructureTemplateId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No salary structure template is configured for this employee, and no Grade/Designation default exists for this company. Please assign one before calculating allowance." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Success!", Data = result };
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to resolve the effective template. Please try again later." }; }
        }
    }
}
