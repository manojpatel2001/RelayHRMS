using Azure;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeIncrementRespository: IEmployeeIncrementRespository
    {
        private readonly HRMSDbContext _db;

        public EmployeeIncrementRespository(HRMSDbContext db) 
        {
            _db = db;
        }
        public async Task<List<IncrementReason>> GetAllIncrementReason()
        {
            try
            {
                var result = await _db.Set<IncrementReason>()
                                      .FromSqlInterpolated($"EXEC GetAllIncrementReason")
                                      .ToListAsync();

                return result ?? new List<IncrementReason>();
            }
            catch (Exception ex)
            {
                return new List<IncrementReason>();
            }
        }
        public async Task<APIResponse> CreateEmployeeIncrementSalary(vmEmployeeIncrementSalary vmEmployeeSalary)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC SP_EmployeeIncrementSalary
                    @Action = {"CREATE"},
                    @EmployeeId = {vmEmployeeSalary.EmployeeId},
                    @CompanyId = {vmEmployeeSalary.CompanyId},
                    @GrossSalary = {vmEmployeeSalary.GrossSalary},
                    @BasicSalary = {vmEmployeeSalary.BasicSalary},
                    @IsPFApplicable = {vmEmployeeSalary.IsPFApplicable},
                    @ReasonId = {vmEmployeeSalary.ReasonId},
                    @EffectedDate = {vmEmployeeSalary.EffectedDate}
                     
                    
            ").ToListAsync();

                var data = result.FirstOrDefault();
                if(data!=null && data.Success>0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = data.ResponseMessage };
                }
                else
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = data.ResponseMessage };

                }
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<APIResponse> GetAllIncrementEmployees(int companyId)
        {
            try
            {
                
                var result = await _db.Set<vmGetAllIncrementEmployees>()
                    .FromSqlInterpolated($@"EXEC [dbo].[GetAllIncrementEmployees] @CompanyId={companyId}")
                    .ToListAsync();

                if (result == null || !result.Any())
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No increment records found",
                        Data = new List<vmGetAllIncrementEmployees>() // Return empty list instead of null
                    };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = $"{result.Count} records fetched successfully"
                };
            }
            catch (Exception ex)
            {
              
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }


        public async Task<APIResponse> DeleteIncrement(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"EXEC SP_EmployeeIncrementSalary
                @Action={"DELETE"},
                @EmployeeId={EmployeeId}")
                    .ToListAsync();

                var data = result.FirstOrDefault();

                if (data != null && data.Success > 0)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = data.ResponseMessage
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = data?.ResponseMessage ?? "No response from database"
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Something went wrong: " + ex.Message
                };
            }
        }

    }
}
