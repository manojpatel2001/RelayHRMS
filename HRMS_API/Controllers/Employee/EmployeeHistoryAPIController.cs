using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeHistoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeHistoryAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpGet("GetEmployeeLifecycleTimeline")]
        public async Task<APIResponse> GetEmployeeLifecycleTimeline(int EmployeeId, int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeHistoryRepository.GetEmployeeLifecycleTimeline(EmployeeId, CompanyId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
