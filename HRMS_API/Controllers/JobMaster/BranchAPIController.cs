using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BranchAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllBranches")]
        public async Task<APIResponse> GetAllBranches()
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetAllBranches(new vmCommonGetById { });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


        [HttpGet("GetBranchById/{id}")]
        public async Task<APIResponse> GetBranchById(int id)
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetBranchById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpGet("GetAllCityByStateId/{stateId}")]
        public async Task<APIResponse> GetAllCityByStateId(int stateId)
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetAllCityByStateId(new vmCommonGetById { Id = stateId });
                if (data == null||!data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateBranch")]
        public async Task<APIResponse> CreateBranch(Branch model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Branch details cannot be null." };

                var exists = await _unitOfWork.BranchRepository.GetAllBranches(new vmCommonGetById { Title = model.BranchName.ToLower() });

                if (exists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.BranchName}' already exists." };

                var existBranch = await _unitOfWork.BranchRepository.CheckExistBranchCode(new vmCommonGetById { Title = model.BranchCode });
                if (existBranch != null)
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with branchCode '{model.BranchCode}' already exists." };

                var result = await _unitOfWork.BranchRepository.CreateBranch(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateBranch")]
        public async Task<APIResponse> UpdateBranch(Branch model)
        {
            try
            {
                if (model == null || model.BranchId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Branch details cannot be null." };

                var check = await _unitOfWork.BranchRepository.GetBranchById(new vmCommonGetById { Id = model.BranchId });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var exists = await _unitOfWork.BranchRepository.GetAllBranches(new vmCommonGetById { Title = model.BranchName.ToLower() });

                if (exists.Any(x => x.BranchId != model.BranchId && x.BranchName.ToLower() == model.BranchName.ToLower()))
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.BranchName}' already exists." };

                var exixtBrnachCode = await _unitOfWork.BranchRepository.CheckExistBranchCode(new vmCommonGetById { Title = model.BranchCode });
                if (exixtBrnachCode != null)
                {
                    if(exixtBrnachCode.BranchId!=model.BranchId)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with BranchCode '{model.BranchCode}' already exists." };

                    }
                }

                var result = await _unitOfWork.BranchRepository.UpdateBranch(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteBranch")]
        public async Task<APIResponse> DeleteBranch(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.BranchRepository.GetBranchById(new vmCommonGetById { Id = model.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var result = await _unitOfWork.BranchRepository.DeleteBranch(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }


        [HttpGet("GetAllBranchByState/{State}")]
        public async Task<APIResponse> GetAllBranchByState(string State)
        {
            try
            {
                //var data = await _unitOfWork.BranchRepository.GetAllAsync(x => x.State.ToLower() == State.ToLower() && x.IsDeleted == false && x.IsEnabled == true);
                //if (data == null || !data.Any())
                //{
                //    return new APIResponse
                //    {
                //        isSuccess = false,
                //        ResponseMessage = "No records found"
                //    };
                //}
                return new APIResponse() { isSuccess = true, Data = null, ResponseMessage = "Records fetched successfully" };
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



        [HttpGet("GetBranchWiseEmpCount")]
        public async Task<APIResponse> GetBranchWiseEmpCount()
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetBranchWiseEmpCount();

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
                    Data = err.Message, // ✅ Set Data to null (not a string)
                    ResponseMessage = $"Error: {err.Message}" // still show message in ResponseMessage
                };
            }
        }

        [HttpGet("GetAllBranchesListByCompanyId/{companyId}")]
        public async Task<APIResponse> GetAllBranchesListByCompanyId(int companyId)
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetAllBranchesListByCompanyId(new vmCommonGetById { CompanyId=companyId});
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpGet("GetBranchesByEmployee")]
        public async Task<APIResponse> GetBranchesByEmployee(int EmpId ,int CompId)
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetBranchesByEmployee(EmpId , CompId);    
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpGet("GetEmployeesByBranchAndUser")]
        public async Task<APIResponse> GetEmployeesByBranchAndUser(int Empid , int CompId , int BranchId)
        {
            try
            {
                var data = await _unitOfWork.BranchRepository.GetEmployeesByBranchAndUser (Empid , CompId , BranchId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
    }
}
