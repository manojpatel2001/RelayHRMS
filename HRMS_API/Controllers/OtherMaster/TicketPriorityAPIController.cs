using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketPriorityAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketPriorityAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTicketPriority")]
        public async Task<APIResponse> GetAllTicketPriority()
        {
            try
            {
                var data = await _unitOfWork.TicketPriorityRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
                if (data == null || !data.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }
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


        [HttpGet("GetByTicketPriorityId/{id}")]
        public async Task<APIResponse> GetByTicketPriorityId(int ticketPriorityId)
        {
            try
            {
                var data = await _unitOfWork.TicketPriorityRepository.GetAsync(x => x.TicketPriorityId == ticketPriorityId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateTicketPriority")]
        public async Task<APIResponse> CreateTicketPriority(TicketPriority ticketPriority)
        {
            try
            {
                if (ticketPriority == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Ticket priority details cannot be null" };
                }

                if (ticketPriority.TicketPriorityId == 0)
                {
                    var isExists = await _unitOfWork.TicketPriorityRepository.GetAllAsync(asd => asd.TicketPriorityName.ToLower().Trim() == ticketPriority.TicketPriorityName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{ticketPriority.TicketPriorityName}' already exists" };
                    }

                    ticketPriority.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.TicketPriorityRepository.CreateTicketPriority(ticketPriority);
                    if (result.Id > 0)
                    {
                        var newTicketPriority = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == result.Id);

                        return new APIResponse() { isSuccess = true, Data = newTicketPriority, ResponseMessage = "The record has been saved successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to add record" };

                }
                else
                {
                    var isExists = await _unitOfWork.TicketPriorityRepository.GetAllAsync(asd => asd.TicketPriorityName.ToLower().Trim() == ticketPriority.TicketPriorityName.ToLower().Trim() && asd.TicketPriorityId != ticketPriority.TicketPriorityId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{ticketPriority.TicketPriorityName}' already exists" };
                    }

                    var checkValidId = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == ticketPriority.TicketPriorityId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (checkValidId == null)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                    }


                    ticketPriority.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.TicketPriorityRepository.UpdateTicketPriority(ticketPriority);
                    if (result.Id > 0)
                    {
                        var updatedTicketPriority = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == ticketPriority.TicketPriorityId);

                        return new APIResponse() { isSuccess = true, Data = updatedTicketPriority, ResponseMessage = "The record has been updated successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };

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

        [HttpPut("UpdateTicketPriority")]
        public async Task<APIResponse> UpdateTicketPriority(TicketPriority ticketPriority)
        {
            try
            {
                if (ticketPriority == null || ticketPriority.TicketPriorityId == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Ticket priority details cannot be null" };
                }


                var isExists = await _unitOfWork.TicketPriorityRepository.GetAllAsync(asd => asd.TicketPriorityName.ToLower().Trim() == ticketPriority.TicketPriorityName.ToLower().Trim() && asd.TicketPriorityId != ticketPriority.TicketPriorityId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{ticketPriority.TicketPriorityName}' already exists" };
                }

                var checkValidId = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == ticketPriority.TicketPriorityId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }


                ticketPriority.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.TicketPriorityRepository.UpdateTicketPriority(ticketPriority);
                if (result.Id > 0)
                {
                    var updatedTicketPriority = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == ticketPriority.TicketPriorityId);

                    return new APIResponse() { isSuccess = true, Data = updatedTicketPriority, ResponseMessage = "The record has been updated successfully" };

                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };

            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update records, Please try again later!"
                };
            }
        }

        [HttpDelete("DeleteTicketPriority")]
        public async Task<APIResponse> DeleteTicketPriority(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                if (deleteRecordVM == null || deleteRecordVM.Id == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var checkValidId = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == deleteRecordVM.Id && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }

                deleteRecordVM.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.TicketPriorityRepository.DeleteTicketPriority(deleteRecordVM);
                if (result.Id > 0)
                {
                    var deletedTicketType = await _unitOfWork.TicketPriorityRepository.GetAsync(asd => asd.TicketPriorityId == deleteRecordVM.Id);

                    return new APIResponse() { isSuccess = true, Data = deletedTicketType, ResponseMessage = "The record has been deleted successfully" };

                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to delete record" };

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

    }
}
