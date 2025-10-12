using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManpowerRequisitionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManpowerRequisitionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetAllManpowerRequisitions")]
        public async Task<APIResponse> GetAllManpowerRequisitions(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllManpowerRequisitions(commonParameter);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No manpower requisitions found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Manpower requisitions fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve manpower requisitions. Please try again later." };
            }
        }

        [HttpGet("GetDropDownForManpower/{CompanyId}")]
        public async Task<APIResponse> GetDropDownForManpower(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetDropDownForManpower(CompanyId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
     
        [HttpPost("CreateManpowerRequisition")]
        public async Task<APIResponse> CreateManpowerRequisition([FromBody] ManpowerRequisition model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Manpower requisition details cannot be null." };

                // Set default values for required fields if needed
                model.CreatedDate = DateTime.UtcNow;
                model.IsEnabled = true;
                model.IsDeleted = false;

                var result = await _unitOfWork.ManpowerRequisitionRepository.CreateManpowerRequisition(model);
                if (result.Success > 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = result.ResponseMessage
                    };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create manpower requisition. Please try again later." };
            }
        }

        [HttpPost("UpdateManpowerRequisition")]
        public async Task<APIResponse> UpdateManpowerRequisition([FromBody] ManpowerRequisition model)
        {
            try
            {
                if (model == null || model.ManpowerRequisitionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Manpower requisition details cannot be null." };

                // Set updated date and user
                model.UpdatedDate = DateTime.UtcNow;

                var result = await _unitOfWork.ManpowerRequisitionRepository.UpdateManpowerRequisition(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update manpower requisition. Please try again later." };
            }
        }

        [HttpDelete("DeleteManpowerRequisition")]
        public async Task<APIResponse> DeleteManpowerRequisition(DeleteRecordVM model)
        {
            try
            {
                if (model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Manpower requisition ID cannot be zero." };

                var result = await _unitOfWork.ManpowerRequisitionRepository.DeleteManpowerRequisition(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete manpower requisition. Please try again later." };
            }
        }

        [HttpPost("GetAllSerialNo")]
        public async Task<APIResponse> GetAllSerialNo(CommonParameter model)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllSerialNo(model);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpGet("GetManpowerRequisitionByManpowerRequisitionId/{ManpowerRequisitionId}")]
        public async Task<APIResponse> GetManpowerRequisitionByManpowerRequisitionId(int ManpowerRequisitionId)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetManpowerRequisitionByManpowerRequisitionId(ManpowerRequisitionId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpPost("UpdateJoinningDetails")]
        public async Task<APIResponse> UpdateJoinningDetails(UpdateJoinningDetailsModel model)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.UpdateJoinningDetails( model);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

    }

}
