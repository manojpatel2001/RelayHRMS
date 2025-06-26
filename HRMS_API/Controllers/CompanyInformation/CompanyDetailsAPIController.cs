using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.Helper;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace HRMS_API.Controllers.CompanyInformation
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _clientFactory;

        public CompanyDetailsAPIController(IUnitOfWork unitOfWork, IHttpClientFactory clientFactory)
        {
            _unitOfWork = unitOfWork;
            _clientFactory = clientFactory;
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

        [HttpGet("GetAllCompanyDetailsList")]
        public async Task<APIResponse> GetAllCompanyDetailsList()
        {
            try
            {
                var data = await _unitOfWork.CompanyDetailsRepository.GetAllCompanyDetailsList();
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

        [HttpPost("ChangeCompanyLogo")]
        public async Task<APIResponse> ChangeCompanyLogo(vmChangeCompanyLogo model)
        {
            try
            {
                if (model == null || model.companyId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };
                var check = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId((int)model.companyId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                if (model.LogoFile != null || model.LogoFile.Length>0)
                {

                    var folder = $"uploads/companylogo";
                    var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, model.LogoFile, folder, null);
                    model.CompanyLogoUrl = fileUrl;
                }
                var result = await _unitOfWork.CompanyDetailsRepository.UpdateCompanyLogo(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The company logo has been changed successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to change company logo. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to change company logo. Please try again later." };
            }
        }

        [HttpPost("UpdateLetterHead")]
        public async Task<APIResponse> UpdateLetterHead(vmUploadHeader model)
        {
            try
            {
                if (model == null || model.companyId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };
                var check = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId((int)model.companyId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                if (model.LetterHeadHeaderFile != null || model.LetterHeadHeaderFile.Length > 0)
                {

                    var folder = $"uploads/companyletterhead";
                    var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, model.LetterHeadHeaderFile, folder, null);
                    model.LetterHeadHeaderUrl = fileUrl;
                }
                if (model.LetterHeadFooterFile != null || model.LetterHeadFooterFile.Length > 0)
                {

                    var folder = $"uploads/companyletterhead";
                    var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, model.LetterHeadFooterFile, folder, null);
                    model.LetterHeadFooterUrl = fileUrl;
                }
                var result = await _unitOfWork.CompanyDetailsRepository.UpdateLetterHead(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The letter head has been uploaded successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to upload letter head. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to upload letter head. Please try again later." };
            }
        }

        [HttpPost("UpdateDigitalSignature")]
        public async Task<APIResponse> UpdateDigitalSignature(vmDigitalSignature model)
        {
            try
            {
                if (model == null || model.CompanyId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };
                var check = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId((int)model.CompanyId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                if (model.DigitalSignatureFile != null || model.DigitalSignatureFile.Length > 0)
                {

                    var folder = $"uploads/companydigitalsignature";
                    var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, model.DigitalSignatureFile, folder, null);
                    model.DigitalSignatureUrl = fileUrl;
                }
                // Hash the password before saving it to the database
                if (!string.IsNullOrEmpty(model.DigitalSignaturePassword))
                {
                   model.DigitalSignaturePassword = BCrypt.Net.BCrypt.HashPassword(model.DigitalSignaturePassword);
                }
                var result = await _unitOfWork.CompanyDetailsRepository.UpdateDigitalSignature(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The digital signature has been saved successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save digital signature. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable save digital signature. Please try again later." };
            }
        }

       // [HttpPost("ReadDigitalSignature")]
        //public async Task<APIResponse> ReadDigitalSignature(vmReadDigitalSignature vmReadDigitalSignature)
        //{

        //    try
        //    {
        //        if (vmReadDigitalSignature==null)
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Digital signature cannot be null!" };
        //        var data = await _unitOfWork.CompanyDetailsRepository.GetAsync(x => x.CompanyId == vmReadDigitalSignature.CompanyId);
        //        if (data == null) 
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };
        //        }
                
        //        // Fetch the PFX file from the URL
        //        var client = _clientFactory.CreateClient();
        //        var response = await client.GetAsync(data.DigitalSignatureUrl);

        //        if (!response.IsSuccessStatusCode)
        //            return new APIResponse { isSuccess = false,ResponseMessage = "Digital file not found!" };

        //        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(vmReadDigitalSignature.DigitalPassword, data.DigitalSignaturePassword);
        //        if (!isPasswordCorrect)
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Please enter correct password!" };
        //        }

        //        var pfxBytes = await response.Content.ReadAsByteArrayAsync();

        //        // Load the PFX file
        //        var cert = new X509Certificate2(pfxBytes, vmReadDigitalSignature.DigitalPassword, X509KeyStorageFlags.PersistKeySet);

        //        var certInfo = new
        //        {
        //            Subject = cert.Subject,
        //            Issuer = cert.Issuer,
        //            SerialNumber = cert.SerialNumber,
        //            NotBefore = cert.NotBefore,
        //            NotAfter = cert.NotAfter,
        //            PublicKey = cert.PublicKey.Key.ToXmlString(false)
        //        };

        //        return new APIResponse { isSuccess = true, Data = certInfo, ResponseMessage = "Digital signature retrived successfully!" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable save digital signature. Please try again later." };
        //    }
        //}
    }
}
