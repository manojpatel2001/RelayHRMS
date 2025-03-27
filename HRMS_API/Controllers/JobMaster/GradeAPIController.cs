using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GradeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllGrade")]
        public async Task<APIResponse> GetAllGrade()
        {
            try
            {
                var data = await _unitOfWork.GradeRepository.GetAllAsync();
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


        [HttpGet("GetByGradeId/{id}")]
        public async Task<APIResponse> GetByGradeId(int GradeId)
        {
            try
            {
                var data = await _unitOfWork.GradeRepository.GetAsync(x => x.GradeId == GradeId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateGrade")]
        public async Task<APIResponse> CreateGrade(Grade grade)
        {
            try
            {
                if (grade == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Grade details cannot be null" };
                }

                var isExists = await _unitOfWork.GradeRepository.GetAllAsync(asd => asd.GradeName.ToLower().Trim() == grade.GradeName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{grade.GradeName}' already exists" };
                }
                grade.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.GradeRepository.AddAsync(grade);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = grade, ResponseMessage = "The record has been saved successfully" };
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


        [HttpPut("UpdateGrade")]
        public async Task<APIResponse> UpdateGrade(Grade grade)
        {
            try
            {
                if (grade == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Grade details cannot be null" };
                }

                var oldgrade = await _unitOfWork.GradeRepository.GetAsync(asd => asd.GradeId == grade.GradeId && asd.IsEnabled == true && asd.IsDeleted == false);

                if (oldgrade != null)
                {
                    bool isDeleted = await _unitOfWork.GradeRepository.UpdateGrade(grade);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = grade, ResponseMessage = "The record has been updated successfully" };
                }
                else
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                }

            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update record, Please try again later!"
                };
            }
        }


        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var data = await _unitOfWork.GradeRepository.SoftDelete(DeleteRecord);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = DeleteRecord, ResponseMessage = "The record has been deleted successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to delete records, Please try again later!"
                };
            }
        }

    }
}
