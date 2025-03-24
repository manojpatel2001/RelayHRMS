using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllBranch")]
        public async Task<APIResponse> GetAllBranch()
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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


        [HttpGet("GetByBranchId/{id}")]
        public async Task<APIResponse> GetByBranchId(int BranchId)
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetAsync(x => x.BranchId == BranchId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateBranch")]
        public async Task<APIResponse> CreateBranch(Branch branch)
        {
            try
            {
                if (branch == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Branch details cannot be null" };
                }

                if(branch.BranchId == 0)
                {
                    var isExists = await _unitOfWork.BranchRepository.GetAllAsync(asd => asd.BranchName.ToLower().Trim() == branch.BranchName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{branch.BranchName}' already exists" };
                    }
                    branch.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.BranchRepository.AddAsync(branch);
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = branch, ResponseMessage = "The record has been saved successfully" };
                }
                else
                {
                    var oldBranch = await _unitOfWork.BranchRepository.GetAsync(asd => asd.BranchId == branch.BranchId && asd.IsEnabled == true && asd.IsDeleted == false);

                    if (oldBranch != null)
                    {
                        bool isDeleted = await _unitOfWork.BranchRepository.UpdateBranch(branch);
                        if (!isDeleted)
                        {
                            return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                        }
                        await _unitOfWork.CommitAsync();

                        return new APIResponse() { isSuccess = true, Data = branch, ResponseMessage = "The record has been updated successfully" };
                    }
                    else
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
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


        [HttpPut("UpdateBranch")]
        public async Task<APIResponse> UpdateBranch(Branch branch)
        {
            try
            {
                if (branch == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Branch details cannot be null" };
                }

                var oldBranch = await _unitOfWork.BranchRepository.GetAsync(asd => asd.BranchId == branch.BranchId && asd.IsEnabled == true && asd.IsDeleted == false);

                if (oldBranch != null)
                {
                    bool isDeleted = await _unitOfWork.BranchRepository.UpdateBranch(branch);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = branch, ResponseMessage = "The record has been updated successfully" };
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

                var data = await _unitOfWork.BranchRepository.SoftDelete(DeleteRecord);
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
