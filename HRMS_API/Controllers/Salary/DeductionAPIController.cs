using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeductionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDeduction")]
        public async Task<APIResponse> GetAllDeduction()
        {
            try
            {
                var data = await _unitOfWork.DeductionRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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


        [HttpGet("GetByDeductionId/{id}")]
        public async Task<APIResponse> GetByDeductionId(int EarningId)
        {
            try
            {
                var data = await _unitOfWork.DeductionRepository.GetAsync(x => x.DeductionId == EarningId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateDeduction")]
        public async Task<APIResponse> CreateDeduction(Deduction deduction)
        {
            try
            {
                if (deduction == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Shift details cannot be null" };
                }

                deduction.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.DeductionRepository.AddAsync(deduction);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = deduction, ResponseMessage = "The record has been saved successfully" };

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

        [HttpPut("UpdateDeduction")]
        public async Task<APIResponse> UpdateDeduction(Deduction deduction)
        {
            try
            {
                if (deduction == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Invalid earning details provided."
                    };
                }

                await _unitOfWork.DeductionRepository.UpdateDeduction(deduction);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = deduction,
                    ResponseMessage = "The record has been updated successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update the record, please try again later."
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

                var data = await _unitOfWork.DeductionRepository.SoftDelete(DeleteRecord);
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


        [HttpPost("GetDeductionData")]
        public async Task<APIResponse> GetDeductionData(SearchFilterModel searchFilter)
        {
            try
            {
                var data = await _unitOfWork.DeductionRepository.GetDeductionDataAsync(searchFilter);

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = data, // should be a list/array
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null, // ✅ Set Data to null (not a string)
                    ResponseMessage = $"Error: {err.Message}" // still show message in ResponseMessage
                };
            }
        }


    }
}
