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

        [HttpPost("CreateSkill")]
        public async Task<APIResponse> CreateSkill([FromBody] VmSkill model)
        {
            var response = new APIResponse();

            if (model == null || model.EmployeeId == null)
            {
                response.isSuccess = false;
                response.ResponseMessage = "Invalid skill data.";
                return response;
            }
            try
            {
                var result = await _unitOfWork.employeeProfileSkillRepository.CreateEmpskill(model);
               
                response.isSuccess = true;
                response.ResponseMessage = "Skill created successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred while creating the skill. Details: {ex.Message}";
            }

            return response;
        }


        [HttpPut("UpdateEmpProfileSkill")]
        public async Task<APIResponse> UpdateEmpProfileSkill(VmSkill model)
        {
            try
            {
                if (model == null || model.SkillId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Skill details cannot be null." };


                var result = await _unitOfWork.employeeProfileSkillRepository.UpdateEmpProfileSkill(model);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };


            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeProfileSkill")]
        public async Task<APIResponse> DeleteEmployeeProfileSkill(VmSkill model)
        {
            try
            {
                if (model == null || model.SkillId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };


                var result = await _unitOfWork.employeeProfileSkillRepository.DeleteEmpProfileSkill(model);

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


        [HttpGet("GetAllSkill")]
        public async Task<APIResponse> GetAllSkill()
        {
            try
            {
                var data = await _unitOfWork.employeeProfileSkillRepository.GetAllSkill();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

    }
}
