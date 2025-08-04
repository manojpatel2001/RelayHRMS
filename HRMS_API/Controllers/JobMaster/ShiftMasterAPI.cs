﻿using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftMasterAPI : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftMasterAPI(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllShifts")]
        public async Task<APIResponse> GetAllShifts()
        {
            try
            {
                var data = await _unitOfWork.ShiftMasterRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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

        [HttpPost("CreateShift")]
        public async Task<APIResponse> CreateShift(ShiftMaster shiftDetails)
        {
            try
            {
                if (shiftDetails == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Shift details cannot be null" };
                }


                var isExists = await _unitOfWork.ShiftMasterRepository.GetAllAsync(asd => asd.ShiftName.ToLower().Trim() == shiftDetails.ShiftName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                if (isExists.Any())
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{shiftDetails.ShiftName}' already exists" };
                }
                shiftDetails.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.ShiftMasterRepository.AddAsync(shiftDetails);
                await _unitOfWork.CommitAsync();



                return new APIResponse() { isSuccess = true, Data = shiftDetails, ResponseMessage = "The record has been saved successfully" };

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



        [HttpPost("UpdateShift")]
        public async Task<APIResponse> UpdateShift(ShiftMaster shiftDetails)
        {
            try
            {
                if (shiftDetails == null || shiftDetails.ShiftID <= 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Invalid shift details." };
                }

              
                var existing = await _unitOfWork.ShiftMasterRepository.GetAllAsync(x =>
                    x.ShiftName.ToLower().Trim() == shiftDetails.ShiftName.ToLower().Trim() &&
                    x.ShiftID != shiftDetails.ShiftID &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false
                );

                if (existing.Any())
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = $"Another shift with the name '{shiftDetails.ShiftName}' already exists."
                    };
                }

        
                var current = await _unitOfWork.ShiftMasterRepository.GetAsync(asd => asd.ShiftID == shiftDetails.ShiftID && asd.IsEnabled == true && asd.IsDeleted == false);

                if (current != null)
                {
                    bool isDeleted = await _unitOfWork.ShiftMasterRepository.UpdateShiftMaster(shiftDetails);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = shiftDetails, ResponseMessage = "The record has been updated successfully" };
                }
                else
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                }
                // Update fields
                        }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to update the record. Please try again later."
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

                var data = await _unitOfWork.ShiftMasterRepository.SoftDelete(DeleteRecord);
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
