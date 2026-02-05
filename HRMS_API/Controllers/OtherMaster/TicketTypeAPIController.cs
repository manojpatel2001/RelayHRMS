using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.TicketManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketTypeAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketTypeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTicketTypes/{companyId}")]
        public async Task<APIResponse> GetAllTicketTypes(int companyId)
        {
            try
            {
                var data = await _unitOfWork.TicketTypeRepository.GetAllTicketTypes(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetTicketTypeById/{ticketTypeId}")]
        public async Task<APIResponse> GetTicketTypeById(int ticketTypeId)
        {
            try
            {
                var data = await _unitOfWork.TicketTypeRepository.GetTicketTypeById(ticketTypeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

       

        [HttpPost("CreateTicketType")]
        public async Task<APIResponse> CreateTicketType(TicketType model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket type details cannot be null." };

                var result = await _unitOfWork.TicketTypeRepository.CreateTicketType(model);
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

        [HttpPut("UpdateTicketType")]
        public async Task<APIResponse> UpdateTicketType(TicketType ticketType)
        {
            try
            {
                if (ticketType == null || ticketType.TicketTypeId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket type details cannot be null." };

                var result = await _unitOfWork.TicketTypeRepository.UpdateTicketType(ticketType);
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

        [HttpDelete("DeleteTicketType")]
        public async Task<APIResponse> DeleteTicketType([FromBody] DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.TicketTypeRepository.DeleteTicketType(model);
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
