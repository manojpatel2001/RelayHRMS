using HRMS_Core.DbContext;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeContactAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeContactAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetEmployeeContactByEmployeeId/{employeeId}")]
        public async Task<APIResponse> GetEmployeeContact(string employeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeContactRepository.GetEmployeeContactByEmployeeId(employeeId);
                if (data == null)
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
