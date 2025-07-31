using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyProfileAPIController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public MyProfileAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetEmployeeProfile")]
        public async Task<APIResponse> GetEmployeeProfile([FromForm] int employeeId, [FromForm] int companyId)
        {
            try
            {
                if (employeeId == null || companyId == null )
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "employeeId,companyId are required."
                    };
                }
                var data = await _unitOfWork.myProfileRepository.GetEmployeeProfile(employeeId, companyId);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }
    }
}
