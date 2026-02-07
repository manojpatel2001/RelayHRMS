using HRMS_Core.Employee;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsAnnouncementAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsAnnouncementAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("CreateNewsAnnouncement")]
        public async Task<APIResponse> CreateNewsAnnouncement([FromBody] NewsAnnouncement model)
        {
            try
            {
                if (model == null)
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "NewsAnnouncement details cannot be null."
                    };

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.NewsTitle))
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "News Title is required."
                    };

                if (string.IsNullOrWhiteSpace(model.BranchWiseNewsAnnoun))
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Please select at least one branch."
                    };

                if (string.IsNullOrWhiteSpace(model.NewsDescription))
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Description is required."
                    };

                var result = await _unitOfWork.NewsAnnouncementRepository.CreateNewsAnnouncement(model);

                if (result.Success > 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = result.ResponseMessage
                    };
                }

                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = result.ResponseMessage
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to add record. Please try again later."
                };
            }
        }

        [HttpPut("UpdateNewsAnnouncement")]
        public async Task<APIResponse> UpdateNewsAnnouncement([FromBody] NewsAnnouncement model)
        {
            try
            {
                if (model == null)
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "NewsAnnouncement details cannot be null."
                    };

                if (model.NewsID <= 0)
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Invalid News ID."
                    };

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.NewsTitle))
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "News Title is required."
                    };

                if (string.IsNullOrWhiteSpace(model.BranchWiseNewsAnnoun))
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Please select at least one branch."
                    };

                if (string.IsNullOrWhiteSpace(model.NewsDescription))
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Description is required."
                    };

                var result = await _unitOfWork.NewsAnnouncementRepository.UpdateNewsAnnouncement(model);

                if (result.Success > 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = result.ResponseMessage
                    };
                }

                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = result.ResponseMessage
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to update record. Please try again later."
                };
            }
        }

        [HttpDelete("DeleteNewsAnnouncement")]
        public async Task<APIResponse> DeleteNewsAnnouncement([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Invalid request."
                    };

                var result = await _unitOfWork.NewsAnnouncementRepository.DeleteNewsAnnouncement(request);

                if (result.Success > 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = result.ResponseMessage
                    };
                }

                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = result.ResponseMessage
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to delete record. Please try again later."
                };
            }
        }

        [HttpGet("GetNewsAnnouncement/{CompanyId}")]
        public async Task<APIResponse> GetNewsAnnouncement(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.NewsAnnouncementRepository.GetNewsAnnouncement(CompanyId);
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
    }
}
