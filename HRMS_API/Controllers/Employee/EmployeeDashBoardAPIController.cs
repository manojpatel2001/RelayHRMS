using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDashBoardAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeDashBoardAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetRecentJoinedEmployees")]
        public async Task<APIResponse> GetRecentJoinedEmployees(int Compid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetRecentJoinedEmployees(Compid);
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

        [HttpGet("GetTodayMarriageAnnivarsary")]
        public async Task<APIResponse> GetTodayMarriageAnnivarsary(int Compid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetTodayMarriageAnnivarsary(Compid);
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

        [HttpGet("GetTodayBirthdaysByCompany")]
        public async Task<APIResponse> GetTodayBirthdaysByCompany(int Compid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetTodayBirthdaysByCompany(Compid);
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

        [HttpGet("GetTodayWorkAnniversary")]
        public async Task<APIResponse> GetTodayWorkAnniversary(int Compid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetTodayWorkAnniversary(Compid);
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


        [HttpGet("GetDirectIndirectEmployee")]
        public async Task<APIResponse> GetDirectIndirectEmployee(int Compid, int EmployeeId, string Action)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetDirectIndirectEmp(Compid, EmployeeId, Action);
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
        [HttpGet("Getupcommingholidays")]
        public async Task<APIResponse> Getupcommingholidays(int Compid, int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.Getupcommingholidays(Compid, EmployeeId);
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



        [HttpGet("GetCountDirectOrIndirectEmployees")]
        public async Task<APIResponse> GetCountDirectOrIndirectEmployees([FromQuery] int empid, [FromQuery] int Compid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetCountDirectOrIndirectEmployees(empid, Compid);
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
    }
}
