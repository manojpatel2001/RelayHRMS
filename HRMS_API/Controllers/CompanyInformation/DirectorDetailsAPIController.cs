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
    public class DirectorDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DirectorDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDirectorDetails")]
        public async Task<APIResponse> GetAllDirectorDetails()
        {
            try
            {
                var data = await _unitOfWork.DirectorDetailsRepository.GetAllDirectorDetails();
                if (data == null || !data.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }

        [HttpGet("GetByDirectorDetailsId/{DirectorDetailsId}")]
        public async Task<APIResponse> GetByDirectorDetailsId(int DirectorDetailsId)
        {
            try
            {
                var data = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(DirectorDetailsId);
                if (data == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record, Please try again later!" };
            }
        }

        [HttpGet("GetDirectorDetailsByCompanyId/{CompanyId}")]
        public async Task<APIResponse> GetDirectorDetailsByCompanyId(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.DirectorDetailsRepository.GetDirectorDetailsByCompanyId(CompanyId);
                if (data == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record, Please try again later!" };
            }
        }

        [HttpPost("CreateDirectorDetails")]
        public async Task<APIResponse> CreateDirectorDetails(DirectorDetails directorDetails)
        {
            try
            {
                if (directorDetails == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Director details cannot be null" };
                }

                if (directorDetails.DirectorDetailsId == 0)
                {
                    
                    directorDetails.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.DirectorDetailsRepository.CreateDirectorDetails(directorDetails);
                    if (result.Id > 0)
                    {
                        var newData = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "The record has been saved successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record" };
                }
                else
                {
                    var check = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(directorDetails.DirectorDetailsId);
                    if (check == null)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                    }
                    //var isExists = await _unitOfWork.DirectorDetailsRepository.GetAllAsync(d => d.DirectorDetailsId != directorDetails.DirectorDetailsId && d.DirectorName.ToLower().Trim() == directorDetails.DirectorName.ToLower().Trim()
                    //&& d.IsEnabled == true && d.IsDeleted == false);
                    //if (isExists.Any())
                    //{
                    //    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{directorDetails.DirectorName}' already exists" };
                    //}
                    directorDetails.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.DirectorDetailsRepository.UpdateDirectorDetails(directorDetails);
                    if (result.Id > 0)
                    {
                        var updatedData = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(directorDetails.DirectorDetailsId);
                        return new APIResponse { isSuccess = true, Data = updatedData, ResponseMessage = "The record has been updated successfully" };
                    }
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add/update record, Please try again later!" };
            }
        }

        [HttpPut("UpdateDirectorDetails")]
        public async Task<APIResponse> UpdateDirectorDetails(DirectorDetails directorDetails)
        {
            try
            {
                if (directorDetails == null || directorDetails.DirectorDetailsId == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Director details cannot be null" };
                }

                var check = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(directorDetails.DirectorDetailsId);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }
                //var isExists = await _unitOfWork.DirectorDetailsRepository.GetAllAsync(d => d.DirectorDetailsId != directorDetails.DirectorDetailsId && d.DirectorName.ToLower().Trim() == directorDetails.DirectorName.ToLower().Trim()
                //    && d.IsEnabled == true && d.IsDeleted == false);
                //if (isExists.Any())
                //{
                //    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{directorDetails.DirectorName}' already exists" };
                //}
                directorDetails.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.DirectorDetailsRepository.UpdateDirectorDetails(directorDetails);
                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(directorDetails.DirectorDetailsId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record, Please try again later!" };
            }
        }

        [HttpDelete("DeleteDirectorDetails")]
        public async Task<APIResponse> DeleteDirectorDetails(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var check = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(model.Id);
                if (check == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.DirectorDetailsRepository.DeleteDirectorDetails(model);
                if (result.Id > 0)
                {
                    var deleted = await _unitOfWork.DirectorDetailsRepository.GetByDirectorDetailsId(model.Id);
                    return new APIResponse { isSuccess = true, Data = deleted, ResponseMessage = "The record has been deleted successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record, Please try again later!" };
            }
        }

    }
}
