using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.CompanyInformation
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllCompanyDetails")]
        public async Task<APIResponse> GetAllCompanyDetails()
        {
            try
            {
                var data = await _unitOfWork.CompanyDetailsRepository.GetAllCompanyDetails();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetByCompanyId/{id}")]
        public async Task<APIResponse> GetByCompanyId(int id)
        {
            try
            {
                var data = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateCompanyDetails")]
        public async Task<APIResponse> CreateCompanyDetails(CompanyDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };

                if (model.CompanyId == 0)
                {
                    var exists = await _unitOfWork.CompanyDetailsRepository.GetAllAsync(c =>
                        c.CompanyName.ToLower().Trim() == model.CompanyName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CompanyName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CompanyDetailsRepository.CreateCompanyDetails(model);

                    if (result.Id > 0)
                    {
                        var newCompany = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newCompany, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(model.CompanyId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                    var exists = await _unitOfWork.CompanyDetailsRepository.GetAllAsync(c =>
                        c.CompanyId != model.CompanyId &&
                        c.CompanyName.ToLower().Trim() == model.CompanyName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CompanyName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CompanyDetailsRepository.UpdateCompanyDetails(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(model.CompanyId);
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

        [HttpPut("UpdateCompanyDetails")]
        public async Task<APIResponse> UpdateCompanyDetails(CompanyDetails companyDetails)
        {
            try
            {
                if (companyDetails == null || companyDetails.CompanyId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };

                var check = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(companyDetails.CompanyId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                var isExists = await _unitOfWork.CompanyDetailsRepository.GetAllAsync(x =>
                    x.CompanyId != companyDetails.CompanyId &&
                    x.CompanyName.ToLower().Trim() == companyDetails.CompanyName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name  '{companyDetails.CompanyName}' already exists." };

                companyDetails.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CompanyDetailsRepository.UpdateCompanyDetails(companyDetails);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(companyDetails.CompanyId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update company. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteCompanyDetails")]
        public async Task<APIResponse> DeleteCompanyDetails(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var check = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CompanyDetailsRepository.DeleteCompanyDetails(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete company. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
