using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemeTypeAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public SchemeTypeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllSchemeType")]
        public async Task<APIResponse> GetAllSchemeType()
        {
            try
            {
                var data = await _unitOfWork.SchemeTypeRepository.GetAllSchemeType();
                if (data == null || !data.Any())
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
