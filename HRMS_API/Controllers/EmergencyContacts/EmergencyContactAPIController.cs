using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmergencyContacts
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmergencyContactAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmergencyContacts/{EmployeeId}")]
        public async Task<APIResponse> GetAllEmergencyContacts(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmergencyContactRepository.GetAllEmergencyContacts(new vmCommonGetById {Id= EmployeeId });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }

        [HttpGet("GetEmergencyContactById/{id}")]
        public async Task<APIResponse> GetEmergencyContactById(int id)
        {
            try
            {
                var data = await _unitOfWork.EmergencyContactRepository.GetEmergencyContactById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving record." };
            }
        }

        [HttpPost("CreateEmergencyContact")]
        public async Task<APIResponse> CreateEmergencyContact(EmergencyContact model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Emergency contact details cannot be null." };

                model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmergencyContactRepository.CreateEmergencyContact(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "Record created successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create record." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while creating record." };
            }
        }

        [HttpPut("UpdateEmergencyContact")]
        public async Task<APIResponse> UpdateEmergencyContact(EmergencyContact model)
        {
            try
            {
                if (model == null || model.EmergencyContactId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid emergency contact details." };

                var existing = await _unitOfWork.EmergencyContactRepository.GetEmergencyContactById(new vmCommonGetById { Id = model.EmergencyContactId });
                if (existing == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmergencyContactRepository.UpdateEmergencyContact(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "Record updated successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while updating record." };
            }
        }

        [HttpDelete("DeleteEmergencyContact")]
        public async Task<APIResponse> DeleteEmergencyContact(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid delete request." };

                var existing = await _unitOfWork.EmergencyContactRepository.GetEmergencyContactById(new vmCommonGetById { Id = model.Id });
                if (existing == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmergencyContactRepository.DeleteEmergencyContact(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "Record deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while deleting record." };
            }
        }
    }
}
