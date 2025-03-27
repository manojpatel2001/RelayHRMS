using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.CompilerServices;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet("GetAllDepartment")]
        public async Task<APIResponse> GetAllDepartment()
        {
            try
            {
                var data = await _unitOfWork.DepartmentRepository.GetAllAsync();
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


        [HttpGet("GetBydepartmentId/{id}")]
        public async Task<APIResponse> GetBydepartmentId(int departmentId)
        {
            try
            {
                var data = await _unitOfWork.DepartmentRepository.GetAsync(x => x.DepartmentId == departmentId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateDepartment")]
        public async Task<APIResponse> CreateDepartment(Department department)
        {
            try
            {
                if (department == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Department details cannot be null" };
                }

                if(department.DepartmentId == 0)
                {
                    var isExists = await _unitOfWork.DepartmentRepository.GetAllAsync(asd => asd.DepartmentName.ToLower().Trim() == department.DepartmentName.ToLower().Trim() && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{department.DepartmentName}' already exists" };
                    }
                    department.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.DepartmentRepository.AddAsync(department);
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = department, ResponseMessage = "The record has been saved successfully" };
                }
                else
                {
                    var olddepartment = await _unitOfWork.DepartmentRepository.GetAsync(asd => asd.DepartmentId == department.DepartmentId && asd.IsEnabled == true && asd.IsDeleted == false);

                    if (department != null)
                    {
                        bool isDeleted = await _unitOfWork.DepartmentRepository.UpdateDepartment(department);
                        if (!isDeleted)
                        {
                            return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                        }
                        await _unitOfWork.CommitAsync();

                        return new APIResponse() { isSuccess = true, Data = department, ResponseMessage = "The record has been updated successfully" };
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
        public async Task<APIResponse> UpdateBranch(Department department)
        {
            try
            {
                if (department == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Department details cannot be null" };
                }

                var olddepartment = await _unitOfWork.DepartmentRepository.GetAsync(asd => asd.DepartmentId == department.DepartmentId&& asd.IsEnabled == true && asd.IsDeleted == false);

                if (department != null)
                {
                    bool isDeleted = await _unitOfWork.DepartmentRepository.UpdateDepartment(department);
                    if (!isDeleted)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Record not found. Please select a valid record" };
                    }
                    await _unitOfWork.CommitAsync();

                    return new APIResponse() { isSuccess = true, Data = department, ResponseMessage = "The record has been updated successfully" };
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

                var data = await _unitOfWork.DepartmentRepository.SoftDelete(DeleteRecord);
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
