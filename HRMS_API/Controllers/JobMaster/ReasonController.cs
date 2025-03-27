using HRMS_Core.Master.JobMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.JobMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReasonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReasonController(IUnitOfWork unitOfWork  )
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllDepartment")]
        public async Task<APIResponse> GetAllDepartment()
        {
            try
            {
                var data = await _unitOfWork.DepartmentRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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
