using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS_Core.Master.OtherMaster;
using System.Net.Sockets;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketTypeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTicketType")]
        public async Task<APIResponse> GetAllTicketType()
        {
            try
            {
                var data = await _unitOfWork.TicketTypeRepository.GetAllTicketTypes();
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


        [HttpGet("GetByTicketTypeId/{id}")]
        public async Task<APIResponse> GetByTicketTypeId(int ticketTypeId)
        {
            try
            {
                var data = await _unitOfWork.TicketTypeRepository.GetAsync(x => x.TicketTypeId == ticketTypeId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateTicketType")]
        public async Task<APIResponse> CreateTicketType(TicketType ticketType)
        {
            try
            {
                if (ticketType == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Ticket type details cannot be null" };
                }
                
                if (ticketType.TicketTypeId == 0|| ticketType.TicketTypeId == null)
                {
                    var isExists = await _unitOfWork.TicketTypeRepository.GetAllAsync(asd => asd.TicketTypeName.ToLower().Trim() == ticketType.TicketTypeName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{ticketType.TicketTypeName}' already exists" };
                    }

                    ticketType.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.TicketTypeRepository.CreateTicketType(ticketType);
                    if (result.Id > 0)
                    {
                        var newTicketType = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId == result.Id);

                        return new APIResponse() { isSuccess = true, Data = newTicketType, ResponseMessage = "The record has been saved successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to add record" };

                }
                else
                {
                    var isExists = await _unitOfWork.TicketTypeRepository.GetAllAsync(asd => asd.TicketTypeName.ToLower().Trim() == ticketType.TicketTypeName.ToLower().Trim() && asd.TicketTypeId != ticketType.TicketTypeId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{ticketType.TicketTypeName}' already exists" };
                    }

                    var checkValidId = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId== ticketType.TicketTypeId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (checkValidId == null)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                    }


                    ticketType.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.TicketTypeRepository.UpdateTicketType(ticketType);
                    if (result.Id > 0)
                    {
                        var newTicketType = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId == ticketType.TicketTypeId);

                        return new APIResponse() { isSuccess = true, Data = newTicketType, ResponseMessage = "The record has been updated successfully" };

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

        [HttpPut("UpdateTicketType")]
        public async Task<APIResponse> UpdateTicketType(TicketType ticketType)
        {
            try
            {
                if (ticketType == null || ticketType.TicketTypeId == 0|| ticketType.TicketTypeId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Ticket type details cannot be null" };
                }


                var isExists = await _unitOfWork.TicketTypeRepository.GetAllAsync(asd => asd.TicketTypeName.ToLower().Trim() == ticketType.TicketTypeName.ToLower().Trim() && asd.TicketTypeId != ticketType.TicketTypeId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{ticketType.TicketTypeName}' already exists" };
                }

                var checkValidId = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId == ticketType.TicketTypeId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }


                ticketType.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.TicketTypeRepository.UpdateTicketType(ticketType);
                if (result.Id > 0)
                {
                    var addedTicketType = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId == ticketType.TicketTypeId);

                    return new APIResponse() { isSuccess = true, Data = addedTicketType, ResponseMessage = "The record has been updated successfully" };

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

        [HttpDelete("DeleteTicketType")]
        public async Task<APIResponse> DeleteTicketType(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                if (deleteRecordVM == null || deleteRecordVM.Id == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var checkValidId = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId == deleteRecordVM.Id && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }

                deleteRecordVM.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.TicketTypeRepository.DeleteTicketType(deleteRecordVM);
                if (result.Id > 0)
                {
                    var deletedTicketType = await _unitOfWork.TicketTypeRepository.GetAsync(asd => asd.TicketTypeId == deleteRecordVM.Id);

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
