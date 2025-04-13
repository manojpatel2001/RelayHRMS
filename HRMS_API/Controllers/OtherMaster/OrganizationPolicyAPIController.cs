using HRMS_Core.Helper;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.Migrations;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationPolicyAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrganizationPolicyAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllOrganizationPolicy")]
        public async Task<APIResponse> GetAllOrganizationPolicy()
        {
            try
            {
                var data = await _unitOfWork.OrganizationPolicyRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
                if (data == null|| !data.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }


        [HttpGet("GetByOrganizationPolicyId/{id}")]
        public async Task<APIResponse> GetByOrganizationPolicyId(int OrganizationPolicyId)
        {
            try
            {
                var data = await _unitOfWork.OrganizationPolicyRepository.GetAsync(x => x.OrganizationPolicyId == OrganizationPolicyId && x.IsEnabled == true && x.IsDeleted == false);
                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrive records, Please try again later!"
                };
            }

        }


        [HttpPost("CreateOrganizationPolicy")]
        public async Task<APIResponse> CreateOrganizationPolicy(vmOrganizationPolicy Policy)
        {
            try
            {
                if (Policy == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Organization policy details cannot be null" };
                }
                if (string.Equals(Policy.DocumentUrl, "null", StringComparison.OrdinalIgnoreCase))
                {
                    Policy.DocumentUrl = null;
                }

                if (Policy.OrganizationPolicyId == 0)
                {
                    var isExists = await _unitOfWork.OrganizationPolicyRepository.GetAllAsync(asd => asd.OrganizationPolicyName.ToLower().Trim() == Policy.OrganizationPolicyName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{Policy.OrganizationPolicyName}' already exists" };
                    }

                    if (Policy.DocumentFile != null)
                    {

                        var folder = "uploads/organizationpolicy";
                        var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, Policy.DocumentFile, folder, null);
                        Policy.DocumentUrl= fileUrl;
                    }

                    var result = await _unitOfWork.OrganizationPolicyRepository.CreateOrganizationPolicy(Policy);
                    if(result.Id>0)
                    {
                        var organizationPolicies = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId==result.Id);

                        return new APIResponse() { isSuccess = true, Data = organizationPolicies, ResponseMessage = "The record has been saved successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to add record" };

                }
                else
                {
                    var isExists = await _unitOfWork.OrganizationPolicyRepository.GetAllAsync(asd => asd.OrganizationPolicyName.ToLower().Trim() == Policy.OrganizationPolicyName.ToLower().Trim() && asd.OrganizationPolicyId!=Policy.OrganizationPolicyId &&asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{Policy.OrganizationPolicyName}' already exists" };
                    }

                    var checkValidId = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId == Policy.OrganizationPolicyId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (checkValidId == null)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                    }

                    if (Policy.DocumentFile != null)
                    {

                        var folder = "uploads/organizationpolicy";
                        var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, Policy.DocumentFile, folder, checkValidId.DocumentUrl);
                        Policy.DocumentUrl = fileUrl;
                    }

                    Policy.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.OrganizationPolicyRepository.UpdateOrganizationPolicy(Policy);
                    if (result.Id > 0)
                    {
                        var organizationPolicies = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId == Policy.OrganizationPolicyId);

                        return new APIResponse() { isSuccess = true, Data = organizationPolicies, ResponseMessage = "The record has been updated successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };
                    
                }


            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, Please try again later!"
                };
            }
        }

         [HttpPut("UpdateOrganizationPolicy")]
        public async Task<APIResponse> UpdateOrganizationPolicy(vmOrganizationPolicy Policy)
        {
            try
            {
                if (Policy == null|| Policy.OrganizationPolicyId==0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Organization policy details cannot be null" };
                }

               
                var isExists = await _unitOfWork.OrganizationPolicyRepository.GetAllAsync(asd => asd.OrganizationPolicyName.ToLower().Trim() == Policy.OrganizationPolicyName.ToLower().Trim() && asd.OrganizationPolicyId!=Policy.OrganizationPolicyId &&asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{Policy.OrganizationPolicyName}' already exists" };
                }

                var checkValidId = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId == Policy.OrganizationPolicyId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId==null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }

                if (string.Equals(Policy.DocumentUrl, "null", StringComparison.OrdinalIgnoreCase))
                {
                    Policy.DocumentUrl = null;
                }

                if (Policy.DocumentFile != null )
                {

                    var folder = "uploads/organizationpolicy";
                    var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, Policy.DocumentFile, folder, checkValidId.DocumentUrl);
                    Policy.DocumentUrl = fileUrl;
                }

                Policy.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.OrganizationPolicyRepository.UpdateOrganizationPolicy(Policy);
                if (result.Id > 0)
                {
                    var organizationPolicies = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId == Policy.OrganizationPolicyId);

                    return new APIResponse() { isSuccess = true, Data = organizationPolicies, ResponseMessage = "The record has been updated successfully" };

                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };
                    
                
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update records, Please try again later!"
                };
            }
        }

        [HttpDelete("DeleteOrganizationPolicy")]
        public async Task<APIResponse> DeleteOrganizationPolicy(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                if (deleteRecordVM == null|| deleteRecordVM.Id == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var checkValidId = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId == deleteRecordVM.Id && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }

                deleteRecordVM.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.OrganizationPolicyRepository.DeleteOrganizationPolicy(deleteRecordVM);
                if (result.Id > 0)
                {
                    var organizationPolicies = await _unitOfWork.OrganizationPolicyRepository.GetAsync(asd => asd.OrganizationPolicyId == deleteRecordVM.Id);

                    return new APIResponse() { isSuccess = true, Data = organizationPolicies, ResponseMessage = "The record has been deleted successfully" };

                }
                
               return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to delete record" };
                
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, Please try again later!"
                };
            }
        }




    }
}
