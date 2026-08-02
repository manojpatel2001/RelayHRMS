using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryStructureAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaryStructureAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTemplates")]
        public async Task<APIResponse> GetAllTemplates(int? companyId)
        {
            try { return await _unitOfWork.SalaryStructureRepository.GetAllTemplates(companyId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve templates. Please try again later." }; }
        }

        [HttpGet("GetTemplateById/{salaryStructureTemplateId}")]
        public async Task<APIResponse> GetTemplateById(int salaryStructureTemplateId)
        {
            try { return await _unitOfWork.SalaryStructureRepository.GetTemplateById(salaryStructureTemplateId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve template. Please try again later." }; }
        }

        [HttpPost("CreateTemplate")]
        public async Task<APIResponse> CreateTemplate([FromBody] SalaryStructureTemplate model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.TemplateName))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Template name is required." };
                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.SalaryStructureRepository.CreateTemplate(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create template. Please try again later." }; }
        }

        [HttpPost("UpdateTemplate")]
        public async Task<APIResponse> UpdateTemplate([FromBody] SalaryStructureTemplate model)
        {
            try
            {
                if (model == null || model.SalaryStructureTemplateId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid template." };
                return await _unitOfWork.SalaryStructureRepository.UpdateTemplate(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update template. Please try again later." }; }
        }

        [HttpDelete("DeleteTemplate")]
        public async Task<APIResponse> DeleteTemplate(DeleteRecordVM model)
        {
            try { return await _unitOfWork.SalaryStructureRepository.DeleteTemplate(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete template. Please try again later." }; }
        }

        [HttpPost("CreateComponent")]
        public async Task<APIResponse> CreateComponent([FromBody] SalaryStructureComponent model)
        {
            try
            {
                if (model == null || model.SalaryStructureTemplateId == 0 || string.IsNullOrWhiteSpace(model.ComponentName))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Template and component name are required." };
                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.SalaryStructureRepository.CreateComponent(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create component. Please try again later." }; }
        }

        [HttpPost("UpdateComponent")]
        public async Task<APIResponse> UpdateComponent([FromBody] SalaryStructureComponent model)
        {
            try
            {
                if (model == null || model.SalaryStructureComponentId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid component." };
                return await _unitOfWork.SalaryStructureRepository.UpdateComponent(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update component. Please try again later." }; }
        }

        [HttpDelete("DeleteComponent")]
        public async Task<APIResponse> DeleteComponent(DeleteRecordVM model)
        {
            try { return await _unitOfWork.SalaryStructureRepository.DeleteComponent(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete component. Please try again later." }; }
        }

        [HttpGet("GetComponentsByTemplateId/{salaryStructureTemplateId}")]
        public async Task<APIResponse> GetComponentsByTemplateId(int salaryStructureTemplateId)
        {
            try { return await _unitOfWork.SalaryStructureRepository.GetComponentsByTemplateId(salaryStructureTemplateId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve components. Please try again later." }; }
        }
    }
}
