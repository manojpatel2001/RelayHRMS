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
    public class TicketPriorityAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketPriorityAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTicketPriority/{companyId}")]
        public async Task<APIResponse> GetAllTicketPriority(int companyId)
        {
            try
            {
                var data = await _unitOfWork.TicketPriorityRepository.GetAllTicketPriority(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetTicketPriorityById/{ticketPriorityId}")]
        public async Task<APIResponse> GetTicketPriorityById(int ticketPriorityId)
        {
            try
            {
                var data = await _unitOfWork.TicketPriorityRepository.GetTicketPriorityById(ticketPriorityId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateTicketPriority")]
        public async Task<APIResponse> CreateTicketPriority([FromBody] TicketPriority model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket priority details cannot be null." };

                var result = await _unitOfWork.TicketPriorityRepository.CreateTicketPriority(model);
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

        [HttpPut("UpdateTicketPriority")]
        public async Task<APIResponse> UpdateTicketPriority([FromBody] TicketPriority ticketPriority)
        {
            try
            {
                if (ticketPriority == null || ticketPriority.TicketPriorityId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket priority details cannot be null." };

                var result = await _unitOfWork.TicketPriorityRepository.UpdateTicketPriority(ticketPriority);
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

        [HttpDelete("DeleteTicketPriority")]
        public async Task<APIResponse> DeleteTicketPriority([FromBody] DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.TicketPriorityRepository.DeleteTicketPriority(model);
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
