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
    public class PageMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PageMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllPageMaster")]
        public async Task<APIResponse> GetAllPageMaster()
        {
            try
            {
                var data = await _unitOfWork.PageMasterRepository.GetAllPageMaster();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpGet("GetAllMenuPages")]
        public async Task<APIResponse> GetAllMenuPages()
        {
            try
            {
                var data = await _unitOfWork.PageMasterRepository.GetAllMenuPages();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetPageMasterById/{pageMasterId}")]
        public async Task<APIResponse> GetPageMasterById(int pageMasterId)
        {
            try
            {
                var data = await _unitOfWork.PageMasterRepository.GetPageMasterById(pageMasterId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreatePageMaster")]
        public async Task<APIResponse> CreatePageMaster(PageMaster model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Page Master details cannot be null." };

                if (model.PageMasterId == 0)
                {
                    var exists = await _unitOfWork.PageMasterRepository.GetAllAsync(p =>
                        p.PageName.ToLower().Trim() == model.PageName.ToLower().Trim() &&
                        p.IsDeleted == false &&p.IsEnabled==true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.PageName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.PageMasterRepository.CreatePageMaster(model);

                    if (result.Id > 0)
                    {
                        var newPageMaster = await _unitOfWork.PageMasterRepository.GetPageMasterById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newPageMaster, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.PageMasterRepository.GetPageMasterById(model.PageMasterId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PageMaster record." };

                    var exists = await _unitOfWork.PageMasterRepository.GetAllAsync(p =>
                        p.PageMasterId != model.PageMasterId &&
                        p.PageName.ToLower().Trim() == model.PageName.ToLower().Trim() &&
                        p.IsDeleted == false && p.IsDeleted == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.PageName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.PageMasterRepository.UpdatePageMaster(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.PageMasterRepository.GetPageMasterById(model.PageMasterId);
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

        [HttpPut("UpdatePageMaster")]
        public async Task<APIResponse> UpdatePageMaster(PageMaster pageMaster)
        {
            try
            {
                if (pageMaster == null || pageMaster.PageMasterId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "PageMaster details cannot be null." };

                var check = await _unitOfWork.PageMasterRepository.GetPageMasterById(pageMaster.PageMasterId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PageMaster record." };

                var isExists = await _unitOfWork.PageMasterRepository.GetAllAsync(x =>
                    x.PageMasterId != pageMaster.PageMasterId &&
                    x.PageName.ToLower().Trim() == pageMaster.PageName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{pageMaster.PageName}' already exists." };

                pageMaster.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PageMasterRepository.UpdatePageMaster(pageMaster);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.PageMasterRepository.GetPageMasterById(pageMaster.PageMasterId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update PageMaster. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeletePageMaster")]
        public async Task<APIResponse> DeletePageMaster(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var check = await _unitOfWork.PageMasterRepository.GetPageMasterById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PageMaster record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PageMasterRepository.DeletePageMaster(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete PageMaster. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
