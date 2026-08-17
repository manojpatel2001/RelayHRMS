using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyStatutorySettingAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyStatutorySettingAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public async Task<APIResponse> GetAll()
        {
            try { return await _unitOfWork.CompanyStatutorySettingRepository.GetAll(); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve statutory settings. Please try again later." }; }
        }

        [HttpGet("GetByCompanyId/{companyId}")]
        public async Task<APIResponse> GetByCompanyId(int companyId)
        {
            try
            {
                var result = await _unitOfWork.CompanyStatutorySettingRepository.GetByCompanyId(companyId);
                return result != null
                    ? new APIResponse { isSuccess = true, ResponseMessage = "Success!", Data = result }
                    : new APIResponse { isSuccess = false, ResponseMessage = "No statutory setting configured for this company." };
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve statutory setting. Please try again later." }; }
        }

        [HttpPost("Create")]
        public async Task<APIResponse> Create([FromBody] CompanyStatutorySetting model)
        {
            try
            {
                if (model == null || model.CompanyId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company is required." };
                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.CompanyStatutorySettingRepository.Create(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create statutory setting. Please try again later." }; }
        }

        [HttpPost("Update")]
        public async Task<APIResponse> Update([FromBody] CompanyStatutorySetting model)
        {
            try
            {
                if (model == null || model.CompanyStatutorySettingId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid setting." };
                return await _unitOfWork.CompanyStatutorySettingRepository.Update(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update statutory setting. Please try again later." }; }
        }

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM model)
        {
            try { return await _unitOfWork.CompanyStatutorySettingRepository.Delete(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete statutory setting. Please try again later." }; }
        }
    }
}
