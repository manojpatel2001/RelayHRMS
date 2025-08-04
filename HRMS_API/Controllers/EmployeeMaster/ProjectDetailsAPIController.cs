using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllProjectDetails/{CompanyId}")]
        public async Task<APIResponse> GetAllProjectDetails(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.ProjectDetailsRepository.GetAllProjectDetails(new vmCommonGetById() { Id= CompanyId });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetProjectDetailById/{id}")]
        public async Task<APIResponse> GetProjectDetailById(int id)
        {
            try
            {
                var data = await _unitOfWork.ProjectDetailsRepository.GetProjectDetailById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateProjectDetail")]
        public async Task<APIResponse> CreateProjectDetail(ProjectDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Project details cannot be null." };

                var exists = await _unitOfWork.ProjectDetailsRepository.GetAllProjectDetails(new vmCommonGetById { Title = model.ProjectName.ToLower() });
                if (exists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.ProjectName}' already exists." };

                model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ProjectDetailsRepository.CreateProjectDetail(model);

                return result.Id > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." }
                    : new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateProjectDetail")]
        public async Task<APIResponse> UpdateProjectDetail(ProjectDetails model)
        {
            try
            {
                if (model == null || model.ProjectDetailsId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Project details cannot be null." };

                var check = await _unitOfWork.ProjectDetailsRepository.GetProjectDetailById(new vmCommonGetById { Id = model.ProjectDetailsId });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var exists = await _unitOfWork.ProjectDetailsRepository.GetAllProjectDetails(new vmCommonGetById { Title = model.ProjectName.ToLower() });
                if (exists.Any(x => x.ProjectDetailsId != model.ProjectDetailsId))
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.ProjectName}' already exists." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ProjectDetailsRepository.UpdateProjectDetail(model);

                return result.Id > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." }
                    : new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteProjectDetail")]
        public async Task<APIResponse> DeleteProjectDetail(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.ProjectDetailsRepository.GetProjectDetailById(new vmCommonGetById { Id = model.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ProjectDetailsRepository.DeleteProjectDetail(model);

                return result.Id > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." }
                    : new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
