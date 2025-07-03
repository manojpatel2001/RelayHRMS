using HRMS_Core.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompOffAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompOffAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("CompOffDetails")]

        public async Task<APIResponse> CompOffDetails(Comp_Off_Details COA)
        {
            try
            {
                var isSaved = await _unitOfWork.CompOffDetailsRepository.InsertCompOffAsync(COA);

                if (!isSaved)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to insert Comp Off details." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }





        }



    }
}
