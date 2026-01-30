using HRMS_API.Services;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProfileSkillAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeProfileSkillAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }

        [HttpGet("GetAllEmployeeProfileSkills/{employeeId}")]
        public async Task<APIResponse> GetAllEmployeeProfileSkills(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.employeeProfileSkillRepository.GetAllEmployeeProfile_Skills(employeeId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("CreateEmployeeProfileSkill")]
        public async Task<APIResponse> CreateEmployeeProfileSkill(EmployeeProfile_Skill model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee skill details cannot be null." };
                var result = await _unitOfWork.employeeProfileSkillRepository.CreateEmployeeProfileSkill(model);
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

        [HttpPut("UpdateEmployeeProfileSkill")]
        public async Task<APIResponse> UpdateEmployeeProfileSkill(EmployeeProfile_Skill model)
        {
            try
            {
                if (model == null || model.EmployeeProfile_SkillId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee skill details cannot be null." };
                var result = await _unitOfWork.employeeProfileSkillRepository.UpdateEmployeeProfileSkill(model);
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

        [HttpDelete("DeleteEmployeeProfileSkill")]
        public async Task<APIResponse> DeleteEmployeeProfileSkill(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var result = await _unitOfWork.employeeProfileSkillRepository.DeleteEmployeeProfileSkill(model);
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
