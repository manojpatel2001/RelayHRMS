using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllSkillMasters")]
        public async Task<APIResponse> GetAllSkillMasters()
        {
            try
            {
                var data = await _unitOfWork.SkillMasterRepository.GetAllSkillMasters();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("CreateSkillMaster")]
        public async Task<APIResponse> CreateSkillMaster(SkillMaster model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Skill details cannot be null." };
                var result = await _unitOfWork.SkillMasterRepository.CreateSkillMaster(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateSkillMaster")]
        public async Task<APIResponse> UpdateSkillMaster(SkillMaster model)
        {
            try
            {
                if (model == null || model.SkillMasterId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Skill details cannot be null." };
                var result = await _unitOfWork.SkillMasterRepository.UpdateSkillMaster(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteSkillMaster")]
        public async Task<APIResponse> DeleteSkillMaster(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var result = await _unitOfWork.SkillMasterRepository.DeleteSkillMaster(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
