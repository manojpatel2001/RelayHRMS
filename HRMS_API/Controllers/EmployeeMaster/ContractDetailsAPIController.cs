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
    public class ContractDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContractDetailsAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllContractDetails/{EmployeeId}")]
        public async Task<APIResponse> GetAllContractDetails(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.ContractDetailsRepository.GetAllContractDetails(new vmCommonGetById() { Id= EmployeeId });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetContractDetailById/{id}")]
        public async Task<APIResponse> GetContractDetailById(int id)
        {
            try
            {
                var data = await _unitOfWork.ContractDetailsRepository.GetContractDetailById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateContractDetail")]
        public async Task<APIResponse> CreateContractDetail(ContractDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Contract detail cannot be null." };

                model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ContractDetailsRepository.CreateContractDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateContractDetail")]
        public async Task<APIResponse> UpdateContractDetail(ContractDetails model)
        {
            try
            {
                if (model == null || model.ContractDetailsId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Contract detail cannot be null." };

                var check = await _unitOfWork.ContractDetailsRepository.GetContractDetailById(new vmCommonGetById { Id = model.ContractDetailsId });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ContractDetailsRepository.UpdateContractDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteContractDetail")]
        public async Task<APIResponse> DeleteContractDetail(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.ContractDetailsRepository.GetContractDetailById(new vmCommonGetById { Id = model.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.ContractDetailsRepository.DeleteContractDetail(model);

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
