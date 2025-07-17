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
  
        public class BusinessSegmentAPIController : ControllerBase
        {
            private readonly IUnitOfWork _unitOfWork;

            public BusinessSegmentAPIController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            [HttpGet("GetAllBusinessSegments")]
            public async Task<APIResponse> GetAllBusinessSegments()
            {
                try
                {
                    var data = await _unitOfWork.BusinessSegmentRepository.GetAllBusinessSegments(new vmCommonGetById());
                    if (data == null || !data.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                    return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records." };
                }
            }

            [HttpGet("GetBusinessSegmentById/{id}")]
            public async Task<APIResponse> GetBusinessSegmentById(int id)
            {
                try
                {
                    var data = await _unitOfWork.BusinessSegmentRepository.GetBusinessDetailById(new vmCommonGetById { Id = id });
                    if (data == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                    return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record." };
                }
            }

            [HttpPost("CreateBusinessSegment")]
            public async Task<APIResponse> CreateBusinessSegment(BusinessSegment model)
            {
                try
                {
                    if (model == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Details cannot be null." };

                    var exists = await _unitOfWork.BusinessSegmentRepository.GetAllBusinessSegments(new vmCommonGetById { Title = model.BusinessSegmentName.ToString().ToLower() });
                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.BusinessSegmentName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.BusinessSegmentRepository.CreateBusinessDetail(model);

                    if (result.Id > 0)
                        return new APIResponse { isSuccess = true, ResponseMessage = "Record added successfully." };

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unexpected error occurred." };
                }
            }

            [HttpPut("UpdateBusinessSegment")]
            public async Task<APIResponse> UpdateBusinessSegment(BusinessSegment model)
            {
                try
                {
                    if (model == null || model.BusinessSegmentId == 0)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Invalid input." };

                    var existing = await _unitOfWork.BusinessSegmentRepository.GetBusinessDetailById(new vmCommonGetById { Id = model.BusinessSegmentId });
                    if (existing == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                    var duplicate = await _unitOfWork.BusinessSegmentRepository.GetAllBusinessSegments(new vmCommonGetById { Title = model.BusinessSegmentName.ToString().ToLower() });
                    if (duplicate.Any(x => x.BusinessSegmentId != model.BusinessSegmentId))
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.BusinessSegmentName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.BusinessSegmentRepository.UpdateBusinessDetail(model);

                    if (result.Id > 0)
                        return new APIResponse { isSuccess = true, ResponseMessage = "Record updated successfully." };

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unexpected error occurred." };
                }
            }

            [HttpDelete("DeleteBusinessSegment")]
            public async Task<APIResponse> DeleteBusinessSegment(DeleteRecordVM model)
            {
                try
                {
                    if (model == null || model.Id == 0)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Invalid delete request." };

                    var existing = await _unitOfWork.BusinessSegmentRepository.GetBusinessDetailById(new vmCommonGetById { Id = model.Id });
                    if (existing == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                    model.DeletedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.BusinessSegmentRepository.DeleteBusinessDetail(model);

                    if (result.Id > 0)
                        return new APIResponse { isSuccess = true, ResponseMessage = "Record deleted successfully." };

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record." };
                }
                catch (Exception ex)
                {
                    return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unexpected error occurred." };
                }
            }
        }
}

