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
    public class DesignationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [HttpGet("GetAllDesignation")]
        public async Task<APIResponse> GetAllDesignation()
        {
            try
            {
                var data = await _unitOfWork.DesignationRepository.GetAllAsync();
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


        [HttpGet("GetByDesignationId/{id}")]
        public async Task<APIResponse> GetByDesignationId(int DesignationId)
        {
            try
            {
                var data = await _unitOfWork.DesignationRepository.GetAsync(x => x.DesignationId == DesignationId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateDesignation")]
        public async Task<APIResponse> CreateDesignation(Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Designation details cannot be null" };
                }

                if(designation.DesignationId == 0)
                {
                    var isExists = await _unitOfWork.DesignationRepository.GetAllAsync(asd => asd.DesignationName.ToLower().Trim() == designation.DesignationName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{designation.DesignationName}' already exists" };
                    }
                    designation.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.DesignationRepository.AddAsync(designation);
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = designation, ResponseMessage = "The record has been saved successfully" };
                }
                else
                {
                    var oldBranch = await _unitOfWork.DesignationRepository.GetAsync(asd => asd.DesignationId == designation.DesignationId);

                    if (oldBranch != null)
                    {
                        bool isDeleted = await _unitOfWork.DesignationRepository.UpdateDesignation(designation);
                        if (!isDeleted)
                        {
                            return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                        }
                        await _unitOfWork.CommitAsync();

                        return new APIResponse() { isSuccess = true, Data = designation, ResponseMessage = "The record has been updated successfully" };
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


        [HttpPut("UpdateDesignation")]
        public async Task<APIResponse> UpdateDesignation(Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Designation details cannot be null" };
                }

                var oldBranch = await _unitOfWork.DesignationRepository.GetAsync(asd => asd.DesignationId == designation.DesignationId && asd.IsEnabled == true && asd.IsDeleted == false);

                if (oldBranch != null)
                {
                    bool isDeleted = await _unitOfWork.DesignationRepository.UpdateDesignation(designation);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = designation, ResponseMessage = "The record has been updated successfully" };
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

                var data = await _unitOfWork.DesignationRepository.SoftDelete(DeleteRecord);
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

        [HttpGet("GetAllDesignationByCompanyId/{companyId}")]
        public async Task<APIResponse> GetAllDesignationByCompanyId(int companyId)
        {
            try
            {
                var data = await _unitOfWork.DesignationRepository.GetAllAsync(asp => asp.CompanyId == companyId && asp.IsEnabled == true && asp.IsDeleted == false);
                if (data == null || !data.Any())

                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                var newdata = data.Select(x => new
                {
                    DesignationId = x.DesignationId,
                    DesignationName = x.DesignationName


                });

                return new APIResponse { isSuccess = true, Data = newdata, ResponseMessage = "Records fetched successfully." };


            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

    }
}
