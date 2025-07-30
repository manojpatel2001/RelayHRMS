using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.CompilerServices;

namespace HRMS_API.Controllers.JobMaster
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDepartments")]
        public async Task<APIResponse> GetAllDepartments()
        {
            try
            {
                var data = await _unitOfWork.DepartmentRepository.GetAllDepartments(new vmCommonGetById { });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetDepartmentById/{id}")]
        public async Task<APIResponse> GetDepartmentById(int id)
        {
            try
            {
                var data = await _unitOfWork.DepartmentRepository.GetDepartmentById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateDepartment")]
        public async Task<APIResponse> CreateDepartment(Department model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Department details cannot be null." };

                var exists = await _unitOfWork.DepartmentRepository.GetAllDepartments(new vmCommonGetById {Title = model.DepartmentName.ToLower() });

                if (exists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.DepartmentName}' already exists." };

                var existDepartment = await _unitOfWork.DepartmentRepository.CheckExistDepartmentCode(new vmCommonGetById { Title = model.DepartmentCode });
                if (existDepartment != null)
                {
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with department code '{model.DepartmentCode}' already exists." };
                    
                }
                var result = await _unitOfWork.DepartmentRepository.CreateDepartment(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateDepartment")]
        public async Task<APIResponse> UpdateDepartment(Department model)
        {
            try
            {
                if (model == null || model.DepartmentId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Department details cannot be null." };

                var check = await _unitOfWork.DepartmentRepository.GetDepartmentById(new vmCommonGetById { Id = model.DepartmentId });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var exists = await _unitOfWork.DepartmentRepository.GetAllDepartments(new vmCommonGetById {Title = model.DepartmentName.ToLower() });

                if (exists.Any(x => x.DepartmentId != model.DepartmentId && x.DepartmentName.ToLower() == model.DepartmentName.ToLower()))
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.DepartmentName}' already exists." };

                var existDepartment = await _unitOfWork.DepartmentRepository.CheckExistDepartmentCode(new vmCommonGetById { Title = model.DepartmentCode });
                if (existDepartment != null)
                {
                    if (existDepartment.DepartmentId != model.DepartmentId)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with department code '{model.DepartmentCode}' already exists." };
                    }
                }

                var result = await _unitOfWork.DepartmentRepository.UpdateDepartment(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteDepartment")]
        public async Task<APIResponse> DeleteDepartment(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.DepartmentRepository.GetDepartmentById(new vmCommonGetById { Id = model.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var result = await _unitOfWork.DepartmentRepository.DeleteDepartment(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }


        [HttpGet("GetAllDepartmentByCompanyId/{companyId}")]
        public async Task<APIResponse> GetAllDepartmentByCompanyId(int companyId)
        {
            try
            {
                var data = await _unitOfWork.DepartmentRepository.GetAllAsync(asp => asp.CompanyId == companyId && asp.IsEnabled == true && asp.IsDeleted == false);
                if (data == null || !data.Any())
                    
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                var newdata = data.Select(x => new
                {
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.DepartmentName


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
