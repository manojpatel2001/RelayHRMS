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


        [HttpGet("GetBranchNewJoinerDetails/{CompId}/{BranchId}")]
        public async Task<APIResponse> GetBranchNewJoinerDetails(int CompId, int BranchId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetBranchNewJoinerDetails(CompId, BranchId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
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



        [HttpGet("GetRecentJoinedEmployeesForAdmin")]
        public async Task<APIResponse> GetRecentJoinedEmployeesForAdmin()
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetRecentJoinedEmployeesForAdmin();
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

        [HttpGet("GetEmployeeDetails")]
        public async Task<APIResponse> GetEmployeeDetails(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDashboardRepository.GetEmployeeDetails(EmployeeId);
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



        [HttpGet("GetMyteamleave")]
        public async Task<APIResponse> GetMyteamleave([FromQuery] int empid, [FromQuery] int Compid)
        {
            try
            {
                var reperting = await _unitOfWork.EmployeeManageRepository.GetAsync(asp => asp.Id == empid);
                var reportingpersonid = reperting.ReportingManagerId;
                var data = await _unitOfWork.EmployeeDashboardRepository.GetMyteamleave(empid, Compid, reportingpersonid.Value);
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
