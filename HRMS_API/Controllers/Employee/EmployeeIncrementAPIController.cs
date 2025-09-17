using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeIncrementAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeIncrementAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllIncrementReason")]
        public async Task<APIResponse> GetAllIncrementReason()
        {
            try
            {
                var data = await _unitOfWork.EmployeeIncrementRespository.GetAllIncrementReason();
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
