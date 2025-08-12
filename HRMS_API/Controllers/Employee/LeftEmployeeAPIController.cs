﻿using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository.Employee;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeftEmployeeAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeftEmployeeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllLeftEmp")]
        public async Task<APIResponse> GetAllLeftEmp()
        {
            try
            {
                var data = await _unitOfWork.leftEmployeeRepository.GetAllLeftEmployee();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


        [HttpPost("CreateLeftEmp")]
        public async Task<APIResponse> CreateLeftEmp([FromBody]LeftEmployee model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "LeftEmployee details cannot be null." };

                var result = await _unitOfWork.leftEmployeeRepository.CreateLeftEmployee(model);
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };

            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }
        [HttpPut("UpdateLeftEmployee")]
        public async Task<APIResponse> UpdateLeftEmployee([FromBody] LeftEmployee employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }
                int id = employee.LeftID;

                var check = await _unitOfWork.leftEmployeeRepository.GetLeftEmpById(new vmCommonGetById { Id = id });

                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var result = await _unitOfWork.leftEmployeeRepository.UpdateLeftEmployee(employee);

                //if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };

                //return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
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
                int id = DeleteRecord.Id;

                var check = await _unitOfWork.leftEmployeeRepository.GetLeftEmpById(new vmCommonGetById { Id = id });

                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var data = await _unitOfWork.leftEmployeeRepository.DeleteLeftEmployee(DeleteRecord);
                await _unitOfWork.CommitAsync();

                //if (data.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };

                //return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
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

