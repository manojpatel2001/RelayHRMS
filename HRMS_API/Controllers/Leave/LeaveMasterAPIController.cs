using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       

            [HttpGet("")]
        public async Task<APIResponse> GetAllBranch()
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }

    }
}
