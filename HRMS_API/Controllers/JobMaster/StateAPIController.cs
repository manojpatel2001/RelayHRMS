using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StateAPIController : ControllerBase
    {
         private readonly IUnitOfWork _unitOfWork;

            public StateAPIController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            [HttpGet("GetAllState")]
            public async Task<APIResponse> GetAllState()
            {
                try
                {
                    var data = await _unitOfWork.StateRepository.GetAllStates(new vmCommonGetById { });
                    if (data == null || !data.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                    return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
                }
            }

            [HttpGet("GetStateById/{id}")]
            public async Task<APIResponse> GetStateById(int id)
            {
                try
                {
                    var data = await _unitOfWork.StateRepository.GetStateById(new vmCommonGetById { Id = id });
                    if (data == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                    return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
                }
            }

            [HttpPost("CreateState")]
            public async Task<APIResponse> CreateState(State model)
            {
                try
                {
                    if (model == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "State details cannot be null." };

                    var exists = await _unitOfWork.StateRepository.GetAllStates(new vmCommonGetById { Title = model.StateName.ToLower() });

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.StateName}' already exists." };

                    var result = await _unitOfWork.StateRepository.CreateState(model);

                    if (result.Id > 0)
                    {
                        return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
                }
            }

            [HttpPut("UpdateState")]
            public async Task<APIResponse> UpdateState(State model)
            {
                try
                {
                    if (model == null || model.StateId == 0)
                        return new APIResponse { isSuccess = false, ResponseMessage = "State details cannot be null." };

                    var check = await _unitOfWork.StateRepository.GetStateById(new vmCommonGetById { Id = model.StateId });
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                    var exists = await _unitOfWork.StateRepository.GetAllStates(new vmCommonGetById { Title = model.StateName.ToLower() });

                    if (exists.Any(x => x.StateId != model.StateId && x.StateName.ToLower() == model.StateName.ToLower()))
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.StateName}' already exists." };

                    var result = await _unitOfWork.StateRepository.UpdateState(model);

                    if (result.Id > 0)
                    {
                        return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
                }
            }

            [HttpDelete("DeleteState")]
            public async Task<APIResponse> DeleteState(DeleteRecordVM model)
            {
                try
                {
                    if (model == null || model.Id == 0)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                    var check = await _unitOfWork.StateRepository.GetStateById(new vmCommonGetById { Id = model.Id });
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                    var result = await _unitOfWork.StateRepository.DeleteState(model);

                    if (result.Id > 0)
                        return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
                }
            }
        
    }
}
