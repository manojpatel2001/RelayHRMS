using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;

namespace HRMS_API.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarningAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EarningAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEarning")]
        public async Task<APIResponse> GetAllEarning()
        {
            try
            {
                var data = await _unitOfWork.EarningRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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


        [HttpGet("GetByEarningId/{id}")]
        public async Task<APIResponse> GetByEarningId(int EarningId)
        {
            try
            {
                var data = await _unitOfWork.EarningRepository.GetAsync(x => x.EarningId == EarningId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateEarning")]
        public async Task<APIResponse> CreateEarning(Earning earning)
        {
            try
            {
                if (earning == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Earning details cannot be null" };
                }

                earning.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.EarningRepository.AddAsync(earning);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = earning, ResponseMessage = "The record has been saved successfully" };

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

        [HttpPut("UpdateEarning")]
        public async Task<APIResponse> UpdateEarning(Earning earning)
        {
            try
            {
                if (earning == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Invalid earning details provided."
                    };
                }

                await _unitOfWork.EarningRepository.UpdateEarning(earning);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = earning,
                    ResponseMessage = "The record has been updated successfully."
                };
            }
            catch (Exception err)
            {
                // Optionally log the exception here

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

                var data = await _unitOfWork.EarningRepository.SoftDelete(DeleteRecord);
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



        [HttpPost("GetEarningData")]
        public async Task<APIResponse> GetEarningData(SearchFilterModel searchFilter)
        {
            try
            {
                var data = await _unitOfWork.EarningRepository.GetEarningDataAsync(searchFilter);

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
