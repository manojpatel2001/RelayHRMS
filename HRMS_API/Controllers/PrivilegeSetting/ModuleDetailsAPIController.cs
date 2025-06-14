using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.PrivilegeSetting
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModuleDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllModuleDetailsByCompanyId/{companyId}")]
        public async Task<APIResponse> GetAllModuleDetailsByCompanyId(int companyId)
        {
            try
            {
                var data = await _unitOfWork.ModuleDetailsRepository.GetAllModuleDetailsByCompanyId(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetModuleDetailsById/{moduleDetailsId}")]
        public async Task<APIResponse> GetModuleDetailsById(int moduleDetailsId)
        {
            try
            {
                var data = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById(moduleDetailsId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateModuleDetails")]
        public async Task<APIResponse> CreateModuleDetails(ModuleDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Module details cannot be null." };

                if (model.ModuleDetailsId == 0)
                {
                    var exists = await _unitOfWork.ModuleDetailsRepository.GetAllAsync(m =>
                        m.ModuleName.ToLower().Trim() == model.ModuleName.ToLower().Trim() &&
                        m.IsDeleted == false && m.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.ModuleName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.ModuleDetailsRepository.CreateModuleDetails(model);

                    if (result.Id > 0)
                    {
                        var newModuleDetails = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newModuleDetails, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById(model.ModuleDetailsId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid ModuleDetails record." };

                    var exists = await _unitOfWork.ModuleDetailsRepository.GetAllAsync(m =>
                        m.ModuleDetailsId != model.ModuleDetailsId &&
                        m.ModuleName.ToLower().Trim() == model.ModuleName.ToLower().Trim() &&
                        m.IsDeleted == false && m.IsDeleted == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.ModuleName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.ModuleDetailsRepository.UpdateModuleDetails(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById(model.ModuleDetailsId);
                        return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateModuleDetails")]
        public async Task<APIResponse> UpdateModuleDetails(ModuleDetails moduleDetails)
        {
            try
            {
                if (moduleDetails == null || moduleDetails.ModuleDetailsId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Module details cannot be null." };

                var check = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById(moduleDetails.ModuleDetailsId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid ModuleDetails record." };

                var isExists = await _unitOfWork.ModuleDetailsRepository.GetAllAsync(x =>
                    x.ModuleDetailsId != moduleDetails.ModuleDetailsId &&
                    x.ModuleName.ToLower().Trim() == moduleDetails.ModuleName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{moduleDetails.ModuleName}' already exists." };

                moduleDetails.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ModuleDetailsRepository.UpdateModuleDetails(moduleDetails);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById(moduleDetails.ModuleDetailsId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update ModuleDetails. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteModuleDetails")]
        public async Task<APIResponse> DeleteModuleDetails(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.ModuleDetailsRepository.GetModuleDetailsById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid ModuleDetails record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ModuleDetailsRepository.DeleteModuleDetails(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete ModuleDetails. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
