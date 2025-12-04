using HRMS_Core.Master.Scheme;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Scheme
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemeReportingManagerAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SchemeReportingManagerAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("ManageSchemeReportingManagers")]
        public async Task<APIResponse> ManageSchemeReportingManagers(List<SchemeReportingManagerModel> model)
        {
            try
            {
                var data = await _unitOfWork.SchemeReportingManagerRepository.ManageSchemeReportingManagers(model);

                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetSchemeDropdownDetails")]
        public async Task<APIResponse> GetSchemeDropdownDetails()
        {
            try
            {
                var data = await _unitOfWork.SchemeReportingManagerRepository.GetSchemeDropdownDetails();
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"An error occurred: {ex.Message}" };
            }
        }

        [HttpGet("GetReportingByCompanyId")]
        public async Task<APIResponse> GetReportingByCompanyId(int companyId,int?designationId)
        {
            try
            {
                var data = await _unitOfWork.SchemeReportingManagerRepository.GetReportingByCompanyId(companyId, designationId);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"An error occurred: {ex.Message}" };
            }
        }
        [HttpGet("GetAllEmployByDepartmentId")]
        public async Task<APIResponse> GetAllEmployByDepartmentId(int? companyId,int?departmentId)
        {
            try
            {
                var data = await _unitOfWork.SchemeReportingManagerRepository.GetAllEmployByDepartmentId(companyId, departmentId);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"An error occurred: {ex.Message}" };
            }
        }

        [HttpGet("GetDrpSchemeDetailsBySchemeType")]
        public async Task<APIResponse> GetDrpSchemeDetailsBySchemeType(int? schemeTypeId=null)
        {
            try
            {
                var data = await _unitOfWork.SchemeReportingManagerRepository.GetDrpSchemeDetailsBySchemeType(schemeTypeId);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"An error occurred: {ex.Message}" };
            }
        }

    }
}
