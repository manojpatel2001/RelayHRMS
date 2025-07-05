using HRMS_Core.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompOffAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompOffAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("CompOffDetailsApplication")]

        public async Task<APIResponse> CompOffDetailsApplication([FromBody] Comp_Off_Details COA)
        {
            try
            {

                var comoffdata = new Comp_Off_Details
                {
                    CreatedDate = DateTime.Now,
                    CreatedBy= COA.CreatedBy,
                    Cmp_Id= COA.Cmp_Id,
                    Emp_Id= COA.Emp_Id,
                    Rep_Person_Id= COA.Rep_Person_Id,
                    ApplicationDate= COA.ApplicationDate,
                    Extra_Work_Day= COA.Extra_Work_Day,
                    Extra_Work_Hours=COA.Extra_Work_Hours,
                    Application_Status="Pending",
                    ComoffReason=COA.ComoffReason

                };
                var isSaved = await _unitOfWork.CompOffDetailsRepository.InsertCompOffAsync(comoffdata);

                if (!isSaved)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to insert Comp Off details." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Records Added successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add records. Please try again later." };
            }





        }
        [HttpPost("CompOffDetailsApproveorReject")]

        public async Task<APIResponse> CompOffDetailsApproveorReject(int EMP,string Status)
        {
            try
            {
                var isSaved = await _unitOfWork.CompOffDetailsRepository.Updateapproval(EMP, Status);

                if (!isSaved)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to insert Comp Off details." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Records Updated successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update records. Please try again later." };
            }





        }


        [HttpPost("ReportingPersonEmpVise")]
        public async Task<APIResponse> ReportingPersonEmpVise(int Compid,int Empid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false && asd.IsBlocked==false && asd.CompanyId== Compid && asd.Id!= Empid);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }
                var newdata = data.Select(leave => new
                {
                    ReportingPersonId = leave.Id,
                    ReportingPersonName = leave.FullName
                });
                return new APIResponse() { isSuccess = true, Data = newdata, ResponseMessage = "Record fetched successfully" };
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
