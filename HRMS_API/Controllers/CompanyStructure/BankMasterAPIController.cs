using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.CompanyStructure
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankMasterAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public BankMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllBankMaster")]
        public async Task<APIResponse> GetAllBankMaster()
        {
            try
            {
                var data = await _unitOfWork.BankMasterRepository.GetAllBankMaster();
                if (data == null || !data.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }
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


        [HttpGet("GetByBankMasterId/{bankMasterId}")]
        public async Task<APIResponse> GetByBankMasterId(int bankMasterId)
        {
            try
            {
                var data = await _unitOfWork.BankMasterRepository.GetByBankMasterId(bankMasterId);
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


        [HttpPost("CreateBankMaster")]
        public async Task<APIResponse> CreateBankMaster(BankMaster bankMaster)
        {
            try
            {
                if (bankMaster == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Bank master details cannot be null" };
                }

                if (bankMaster.BankMasterId == 0)
                {

                    bankMaster.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.BankMasterRepository.CreateBankMaster(bankMaster);
                    if (result.Id > 0)
                    {
                        var newTicketType = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == result.Id);

                        return new APIResponse() { isSuccess = true, Data = newTicketType, ResponseMessage = "The record has been saved successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to add record" };

                }
                else
                {

                    var checkValidId = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == bankMaster.BankMasterId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (checkValidId == null)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                    }


                    bankMaster.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.BankMasterRepository.UpdateBankMaster(bankMaster);
                    if (result.Id > 0)
                    {
                        var newBank = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == bankMaster.BankMasterId);

                        return new APIResponse() { isSuccess = true, Data = newBank, ResponseMessage = "The record has been updated successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };

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

        [HttpPut("UpdateTicketType")]
        public async Task<APIResponse> UpdateTicketType(BankMaster bankMaster)
        {
            try
            {
                if (bankMaster == null || bankMaster.BankMasterId == 0 || bankMaster.BankMasterId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Bank master details cannot be null" };
                }


                var checkValidId = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == bankMaster.BankMasterId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }


                bankMaster.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.BankMasterRepository.UpdateBankMaster(bankMaster);
                if (result.Id > 0)
                {
                    var newBank = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == bankMaster.BankMasterId);

                    return new APIResponse() { isSuccess = true, Data = newBank, ResponseMessage = "The record has been updated successfully" };

                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update records, Please try again later!"
                };
            }
        }

        [HttpDelete("DeleteBankMaster")]
        public async Task<APIResponse> DeleteBankMaster(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                if (deleteRecordVM == null || deleteRecordVM.Id == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var checkValidId = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == deleteRecordVM.Id && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }

                deleteRecordVM.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.BankMasterRepository.DeleteBankMaster(deleteRecordVM);
                if (result.Id > 0)
                {
                    var deletedBank = await _unitOfWork.BankMasterRepository.GetAsync(asd => asd.BankMasterId == deleteRecordVM.Id);

                    return new APIResponse() { isSuccess = true, Data = deletedBank, ResponseMessage = "The record has been deleted successfully" };

                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to delete record" };

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




    }
}

    
