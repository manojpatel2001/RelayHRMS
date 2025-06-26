using HRMS_Core.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInOutController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeInOutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEmpINOut")]
        public async Task<APIResponse> CreateEmpINOut([FromBody] EmployeeInOutRecord model)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOut.CreateEmpInOut(model);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Records have been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to add records. Please try again later."
                };
            }
        }

    }
}
