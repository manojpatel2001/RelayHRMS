using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public TestingAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmailTestData")]
        public async Task<APIResponse> GetAllEmailTestData()
        {
            try
            {
                var data =   await _unitOfWork.EmailReportRepository.GetEmployeeEmailDataGrouped();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }

        [HttpGet("InsertAbsentEmployeeEmailData")]
        public async Task<APIResponse> InsertAbsentEmployeeEmailData()
        {
            try
            {
                   await _unitOfWork.EmailReportRepository.InsertAbsentEmployeeEmailData();
               
                return new APIResponse { isSuccess = true, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }
    }
}
