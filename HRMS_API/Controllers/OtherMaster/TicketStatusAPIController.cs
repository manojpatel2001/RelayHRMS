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
    public class TicketStatusAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketStatusAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTicketStatus")]
        public async Task<APIResponse> GetAllTicketStatus()
        {
            try
            {
                var data = await _unitOfWork.TicketStatusRepository.GetAllTicketStatus();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetTicketStatusById/{ticketStatusId}")]
        public async Task<APIResponse> GetTicketStatusById(int ticketStatusId)
        {
            try
            {
                var data = await _unitOfWork.TicketStatusRepository.GetTicketStatusById(ticketStatusId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateTicketStatus")]
        public async Task<APIResponse> CreateTicketStatus([FromBody] TicketStatus model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket status details cannot be null." };

                var result = await _unitOfWork.TicketStatusRepository.CreateTicketStatus(model);
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

        [HttpPut("UpdateTicketStatus")]
        public async Task<APIResponse> UpdateTicketStatus([FromBody] TicketStatus ticketStatus)
        {
            try
            {
                if (ticketStatus == null || ticketStatus.TicketStatusId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket status details cannot be null." };

                var result = await _unitOfWork.TicketStatusRepository.UpdateTicketStatus(ticketStatus);
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

        [HttpDelete("DeleteTicketStatus")]
        public async Task<APIResponse> DeleteTicketStatus([FromBody] DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.TicketStatusRepository.DeleteTicketStatus(model);
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
