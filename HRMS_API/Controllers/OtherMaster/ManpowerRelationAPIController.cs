using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{

    [Route("api/[controller]")]
    [ApiController]
    public class ManpowerRelationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManpowerRelationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<APIResponse> Create([FromBody] ManpowerRelationModel model)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRelationRepository.CreateManpowerRelation(model);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create relation. Please try again later." };
            }
        }

        [HttpPost("Update")]
        public async Task<APIResponse> Update([FromBody] ManpowerRelationModel model)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRelationRepository.UpdateManpowerRelation(model);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update relation. Please try again later." };
            }
        }

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM delete)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRelationRepository.DeleteManpowerRelation(delete);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete relation. Please try again later." };
            }
        }

        [HttpGet("GetAll/{manpowerRequisitionId}")]
        public async Task<APIResponse> GetAll(int manpowerRequisitionId)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRelationRepository.GetAllManpowerRelation(manpowerRequisitionId);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve relations. Please try again later." };
            }
        }
    }

}
