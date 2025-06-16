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
    public class PagePanelAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagePanelAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllPagePanels")]
        public async Task<APIResponse> GetAllPagePanels()
        {
            try
            {
                var data = await _unitOfWork.PagePanelRepository.GetAllPagePanels();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetPagePanelById/{pagePanelId}")]
        public async Task<APIResponse> GetPagePanelById(int pagePanelId)
        {
            try
            {
                var data = await _unitOfWork.PagePanelRepository.GetPagePanelById(pagePanelId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreatePagePanel")]
        public async Task<APIResponse> CreatePagePanel(PagePanel model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Page Panel details cannot be null." };

                if (model.PagePanelId == 0)
                {
                    var exists = await _unitOfWork.PagePanelRepository.GetAllAsync(p =>
                        p.PagePanelName.ToLower().Trim() == model.PagePanelName.ToLower().Trim() &&
                        p.IsDeleted == false && p.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.PagePanelName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.PagePanelRepository.CreatePagePanel(model);

                    if (result.Id > 0)
                    {
                        var newPagePanel = await _unitOfWork.PagePanelRepository.GetPagePanelById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newPagePanel, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.PagePanelRepository.GetPagePanelById(model.PagePanelId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PagePanel record." };

                    var exists = await _unitOfWork.PagePanelRepository.GetAllAsync(p =>
                        p.PagePanelId != model.PagePanelId &&
                        p.PagePanelName.ToLower().Trim() == model.PagePanelName.ToLower().Trim() &&
                        p.IsDeleted == false && p.IsDeleted == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.PagePanelName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.PagePanelRepository.UpdatePagePanel(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.PagePanelRepository.GetPagePanelById(model.PagePanelId);
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

        [HttpPut("UpdatePagePanel")]
        public async Task<APIResponse> UpdatePagePanel(PagePanel pagePanel)
        {
            try
            {
                if (pagePanel == null || pagePanel.PagePanelId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "PagePanel details cannot be null." };

                var check = await _unitOfWork.PagePanelRepository.GetPagePanelById(pagePanel.PagePanelId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PagePanel record." };

                var isExists = await _unitOfWork.PagePanelRepository.GetAllAsync(x =>
                    x.PagePanelId != pagePanel.PagePanelId &&
                    x.PagePanelName.ToLower().Trim() == pagePanel.PagePanelName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{pagePanel.PagePanelName}' already exists." };

                pagePanel.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PagePanelRepository.UpdatePagePanel(pagePanel);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.PagePanelRepository.GetPagePanelById(pagePanel.PagePanelId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update PagePanel. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeletePagePanel")]
        public async Task<APIResponse> DeletePagePanel(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.PagePanelRepository.GetPagePanelById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid PagePanel record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.PagePanelRepository.DeletePagePanel(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete PagePanel. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
