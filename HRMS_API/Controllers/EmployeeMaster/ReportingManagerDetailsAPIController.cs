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
    public class ReportingManagerDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportingManagerDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllReportingManagerDetails/{employeeId}")]
        public async Task<APIResponse> GetAllReportingManagerDetails(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ReportingManagerDetailsRepository.GetAllReportingManagerDetails(new vmCommonGetById() { Id=employeeId});
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetReportingManagerDetailById/{id}")]
        public async Task<APIResponse> GetReportingManagerDetailById(int id)
        {
            try
            {
                var data = await _unitOfWork.ReportingManagerDetailsRepository.GetReportingManagerDetailById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateReportingManagerDetail")]
        public async Task<APIResponse> CreateReportingManagerDetail(ReportingManagerDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Reporting Manager detail cannot be null." };

                model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ReportingManagerDetailsRepository.CreateReportingManagerDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateReportingManagerDetail")]
        public async Task<APIResponse> UpdateReportingManagerDetail(ReportingManagerDetails model)
        {
            try
            {
                if (model == null || model.ReportingManagerDetailsId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Reporting Manager detail cannot be null." };

                var check = await _unitOfWork.ReportingManagerDetailsRepository.GetReportingManagerDetailById(new vmCommonGetById { Id = model.ReportingManagerDetailsId });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ReportingManagerDetailsRepository.UpdateReportingManagerDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteReportingManagerDetail")]
        public async Task<APIResponse> DeleteReportingManagerDetail(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.ReportingManagerDetailsRepository.GetReportingManagerDetailById(new vmCommonGetById { Id = model.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ReportingManagerDetailsRepository.DeleteReportingManagerDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
