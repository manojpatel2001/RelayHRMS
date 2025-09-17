using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ExportData
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportDataAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExportDataAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllEmployeeExportData/{CompanyId}")]
        public async Task<APIResponse> GetAllEmployeeExportData(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.ExportDataRepository.GetAllEmployeeExportData(CompanyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
    }
}
