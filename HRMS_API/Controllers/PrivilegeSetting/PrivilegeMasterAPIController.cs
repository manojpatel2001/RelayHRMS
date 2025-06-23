using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Core.VM.PrivilegeSetting;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.PrivilegeSetting
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivilegeMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrivilegeMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllPrivilegeMasterByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetAllPrivilegeMasterByCompanyId(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.PrivilegeMasterRepository.GetAllPrivilegeMasterByCompanyId(CompanyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetPrivilegeMasterById/{id}")]
        public async Task<APIResponse> GetPrivilegeMasterById(int id)
        {
            try
            {
                var data = await _unitOfWork.PrivilegeMasterRepository.GetPrivilegeMasterById(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreatePrivilegeMaster")]
        public async Task<APIResponse> CreatePrivilegeMaster(vmPrevilegeMaster model)
        {
            try
            {
                if (model == null|| model.PrivilegeMaster==null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "PrivilegeMaster details cannot be null." };
                        var privilegeMaster = model.PrivilegeMaster;
                        var privilegeDetails = model.PrivilegeDetails;

                    
                var exists = await _unitOfWork.PrivilegeMasterRepository.GetAllAsync(p =>
                        p.PrivilegeName.ToLower().Trim() == privilegeMaster.PrivilegeName.ToLower().Trim()&&p.CompanyId==privilegeMaster.CompanyId &&
                        p.IsDeleted == false && p.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{privilegeMaster.PrivilegeName}' already exists." };
                  
                     privilegeMaster.PrivilegeMasterId = 0;
                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.PrivilegeMasterRepository.CreatePrivilegeMaster(privilegeMaster);
                    
                    if (result.Id > 0)
                    {
                        var newPrivilegeMaster = await _unitOfWork.PrivilegeMasterRepository.GetPrivilegeMasterById((int)result.Id);
                        foreach(var details in privilegeDetails)
                        {
                        details.PrivilegeMasterId = result.Id;
                           var privilegeResult = await _unitOfWork.PrivilegeDetailsRepository.CreatePrivilegeDetails(details);

                        }
                    return new APIResponse { isSuccess = true, Data = newPrivilegeMaster, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                
                
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdatePrivilegeMaster")]
        public async Task<APIResponse> UpdatePrivilegeMaster(PrivilegeMaster privilegeMaster)
        {
            try
            {
                if (privilegeMaster == null || privilegeMaster.PrivilegeMasterId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "PrivilegeMaster details cannot be null." };

                var check = await _unitOfWork.PrivilegeMasterRepository.GetPrivilegeMasterById(privilegeMaster.PrivilegeMasterId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PrivilegeMaster record." };

                var isExists = await _unitOfWork.PrivilegeMasterRepository.GetAllAsync(x =>
                    x.PrivilegeMasterId != privilegeMaster.PrivilegeMasterId &&
                    x.PrivilegeName.ToLower().Trim() == privilegeMaster.PrivilegeName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{privilegeMaster.PrivilegeName}' already exists." };

                privilegeMaster.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PrivilegeMasterRepository.UpdatePrivilegeMaster(privilegeMaster);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.PrivilegeMasterRepository.GetPrivilegeMasterById(privilegeMaster.PrivilegeMasterId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update PrivilegeMaster. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeletePrivilegeMaster")]
        public async Task<APIResponse> DeletePrivilegeMaster(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.PrivilegeMasterRepository.GetPrivilegeMasterById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PrivilegeMaster record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PrivilegeMasterRepository.DeletePrivilegeMaster(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete PrivilegeMaster. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }


        [HttpPost("GetAllPageHierarchyByPrivilegeMasterId")]
        public async Task<APIResponse> GetAllPageHierarchyByPrivilegeMasterId(PageVM pageVM)
        {
            try
            {
                var data = await _unitOfWork.PrivilegeMasterRepository.GetAllPageHierarchyByPrivilegeMasterId(pageVM);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };


                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
    }
}
