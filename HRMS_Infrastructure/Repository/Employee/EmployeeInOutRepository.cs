using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.Ess.InOut;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeInOutRepository : Repository<EmployeeInOutRecord>, IEmployeeInOutRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeInOutRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }


        public async Task<List<AttendanceInOutReportVM>> AttendancefirstInOutReport(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<AttendanceInOutReportVM>()
                    .FromSqlRaw("EXEC AttendanceReport_First_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceInOutReportVM>();
            }
        }

        public async Task<List<AttendanceInOutReportVM>> AttendanceMultipleInOutReport(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<AttendanceInOutReportVM>()
                    .FromSqlRaw("EXEC AttendanceReport_Multiple_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceInOutReportVM>();
            }
        }

        public async Task<APIResponse> CreateEmpInOut(vmInOut Record)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeId", Record.EmployeeId);
                    parameters.Add("@CompanyId", Record.CompanyId);
                    parameters.Add("@Mode", Record.Mode);
                    parameters.Add("@PunchDateTime", Record.PunchDateTime);
                    parameters.Add("@CreatedBy", Record.CreatedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                    await connection.ExecuteAsync(
                        "USP_InsertAttendanceLog",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = null; // Insert operation mein data return nahi hota
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }
        public async Task<APIResponse> GetEmployeeInOutReport(EmployeeInOutFilterVM outFilterVM)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmpId", outFilterVM.EmpId);
                    parameters.Add("@StartDate", outFilterVM.StartDate);
                    parameters.Add("@EndDate", outFilterVM.EndDate);
                    parameters.Add("@RecordType", outFilterVM.RecordType);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                    var reportData = (await connection.QueryAsync<EmployeeInOutReportVM>(
                        "GetEmployeeInOutReport",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )).ToList();

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = reportData; // Directly set the report data in response.Data
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<List<VMInOutRecord>> GetInOutRecord(int empid, string month, string year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", month),
                    new SqlParameter("@year", year)
                };

                var result = await _db.Set<VMInOutRecord>()
                    .FromSqlRaw("EXEC First_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<VMInOutRecord>();
            }
        }



        public async Task<List<vmGetMonthlyAttendanceLog>> GetMonthlyAttendanceLog(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var result = await _db.Set<vmGetMonthlyAttendanceLog>()
                                .FromSqlInterpolated($"EXEC GetMonthlyAttendanceLog  @MonthNumber={vmInOutParameter.MonthNumber}, @Year={vmInOutParameter.year},  @EmployeeId = {vmInOutParameter.EmployeeId},@CompanyId={vmInOutParameter.CompanyId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetMonthlyAttendanceLog>();
            }
        }
        public async Task<List<vmGetMonthlyAttendanceDetails>> GetMonthlyAttendanceDetails(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var result = await _db.Set<vmGetMonthlyAttendanceDetails>()
                                .FromSqlInterpolated($"EXEC GetMonthlyAttendanceDetails  @MonthNumber={vmInOutParameter.MonthNumber}, @Year={vmInOutParameter.year},  @EmployeeId = {vmInOutParameter.EmployeeId},@CompanyId={vmInOutParameter.CompanyId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetMonthlyAttendanceDetails>();
            }
        }
        public async Task<List<vmGetMonthlyAttendanceDetails>> GetDateWiseAttendanceDetails(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var result = await _db.Set<vmGetMonthlyAttendanceDetails>()
                                .FromSqlInterpolated($"EXEC GetDateWiseAttendanceDetails  @FromDate={vmInOutParameter.FromDate}, @ToDate={vmInOutParameter.ToDate},  @EmployeeId = {vmInOutParameter.EmployeeId},@CompanyId={vmInOutParameter.CompanyId},@MemberId={vmInOutParameter.MemberId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetMonthlyAttendanceDetails>();
            }
        }

        public async Task<List<vmGetEmployeesByReportingManager>> GetEmployeesByReportingManager(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<vmGetEmployeesByReportingManager>()
                                .FromSqlInterpolated($"EXEC GetEmployeesByReportingManager  @EmployeeId={EmployeeId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetEmployeesByReportingManager>();
            }
        }
        public async Task<List<VMInOutRecord>> GetMultipleInOutRecordAsync(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<VMInOutRecord>()
                    .FromSqlRaw("EXEC Multiple_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<VMInOutRecord>();
            }
        }

        public async Task<bool> UpdateEmployeeOutTimeAsync(int empId, DateTime forDate, DateTime outTime, string updatedBy)
        {
            try
            {
                string sql = @"
                    EXEC SPUpdateOutTimeEmployee 
                        @Emp_Id = {0}, 
                        @For_Date = {1}, 
                        @Out_Time = {2}, 
                        @UpdatedBy = {3}";

                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql, empId, forDate, outTime, updatedBy);
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<VMCommonResult> CreateAttendanceDetails(AttendanceDetailsViewModel Record)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC SP_AttendanceDetails
                        @Action = {"INSERT"},
                        @EmployeeId = {Record.EmployeeId},
                        @ShiftDate = {Record.ShiftDate},
                        @InTime = {Record.InTime},
                        @OutTime = {Record.OutTime},
                        @WorkingHours = {Record.WorkingHours},                
                        @SalaryDay = {Record.SalaryDay},
                        @CreatedOn = {Record.CreatedOn}
                    
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = null };
            }
        }


        public async Task<VMCommonResult> UpdateAttendanceDetails(AttendanceDetailsViewModel model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC SP_AttendanceDetails
                        @Action = {"UPDATE"},
                         @Id = {model.AttendanceDetailsid},
                        @EmployeeId = {model.EmployeeId},
                        @ShiftDate = {model.ShiftDate},
                        @InTime = {model.InTime},
                        @OutTime = {model.OutTime},
                        @WorkingHours = {model.WorkingHours},              
                        @SalaryDay = {model.SalaryDay},
                        @CreatedOn = {model.CreatedOn}
                    
                ").ToListAsync();
                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<List<EmpInOutReportforAdmin>> GetEmpInOutReportForAdmin(EmpInOutReportFilter filter)
        {
            try
            {


                var result = await _db.Set<EmpInOutReportforAdmin>()
                    .FromSqlInterpolated($@"EXEC [dbo].[GetEmpInOutReportForAdmin]
                @StartDate ={filter.StartDate},
                @EndDate ={filter.EndDate},
                @BranchId ={filter.BranchId},
                @EmployeeId ={filter.EmployeeCodes},
                @CompanyId ={filter.CompanyId}")
                .ToListAsync();
                return result;
            }
            catch
            {
                return new List<EmpInOutReportforAdmin>();
            }
        }


        public async Task<APIResponse> GetAttendanceRegularizationAlerts(CommonParameter model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmpId", model.EmployeeId);
                    parameters.Add("@Month", model.Month);
                    parameters.Add("@Year", model.Year);

                    // Use QueryMultipleAsync to handle multiple result sets
                    using (var multi = await connection.QueryMultipleAsync(
                        "sp_GetAttendanceRegularizationAlerts",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    ))
                    {
                        // Read the first result set (e.g., AttendanceDetails)
                        var attendanceDetails = (await multi.ReadAsync<dynamic>()).ToList();

                        // Read the second result set (e.g., Summary)
                        var summary = (await multi.ReadAsync<dynamic>()).ToList();

                        if ((!attendanceDetails.Any() && !summary.Any()) || (attendanceDetails == null && summary == null))
                        {
                            response.Data = null;
                            response.isSuccess = false;
                            response.ResponseMessage = "No Record found!";
                        }
                        else
                        {
                            response.Data = new
                            {
                                AttendanceDetails = attendanceDetails,
                                Summary = summary
                            };
                            response.isSuccess = true;
                            response.ResponseMessage = "Fetched successfully!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

    }
}
