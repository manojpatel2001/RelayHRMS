using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
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
                var data = await _unitOfWork.StateRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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


        [HttpGet("GetByStateId/{id}")]
        public async Task<APIResponse> GetByStateId(int StateId)
        {
            try
            {
                var data = await _unitOfWork.StateRepository.GetAsync(x => x.StateId == StateId && x.IsEnabled == true && x.IsDeleted == false);
                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrive records, Please try again later!"
                };
            }

        }


        [HttpPost("CreateState")]
        public async Task<APIResponse> CreateState(State state)
        {
            try
            {
                if(state.StateId == 0)
                {
                    if (state == null)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "State details cannot be null" };
                    }

                    var isExists = await _unitOfWork.StateRepository.GetAllAsync(asd => asd.StateName.ToLower().Trim() == state.StateName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{state.StateName}' already exists" };
                    }
                    state.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.StateRepository.AddAsync(state);
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = state, ResponseMessage = "The record has been saved successfully" };
                }
                else
                {
                    var oldState = await _unitOfWork.StateRepository.GetAsync(asd => asd.StateId == state.StateId && asd.IsEnabled == true && asd.IsDeleted == false);

                    if (oldState != null)
                    {
                        bool isDeleted = await _unitOfWork.StateRepository.UpdateState(state);
                        if (!isDeleted)
                        {
                            return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                        }
                        await _unitOfWork.CommitAsync();

                        return new APIResponse() { isSuccess = true, Data = state, ResponseMessage = "The record has been updated successfully" };
                    }
                    else
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                }
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, Please try again later!"
                };
            }

        }


        [HttpPut("UpdateState")]
        public async Task<APIResponse> UpdateState(State state)
        {
            try
            {
                if (state == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "State details cannot be null" };
                }

                var oldState = await _unitOfWork.StateRepository.GetAsync(asd => asd.StateId == state.StateId && asd.IsEnabled == true && asd.IsDeleted == false);

                if (oldState != null)
                {
                    bool isDeleted = await _unitOfWork.StateRepository.UpdateState(state);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = state, ResponseMessage = "The record has been updated successfully" };
                }
                else
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                }

            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update record, Please try again later!"
                };
            }
        }


        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var data = await _unitOfWork.StateRepository.SoftDelete(DeleteRecord);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = DeleteRecord, ResponseMessage = "The record has been deleted successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to delete records, Please try again later!"
                };
            }
        }
    }
}
