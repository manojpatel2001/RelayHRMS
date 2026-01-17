using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class AttendanceRegularizationRepository : Repository<AttendanceRegularization>, IAttendanceRegularizationRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public AttendanceRegularizationRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<List<AttendanceRegularizationVM>> GetAttendanceRegularization(AttendanceRegularizationSearchFilterVM attendance)
        {
            try
            {
                var searchbyParam = new SqlParameter("@SearchBy", (object?)attendance.SearchBy ?? DBNull.Value);
                var searchforParam = new SqlParameter("@SearchValue", (object?)attendance.SearchValue ?? DBNull.Value);
                var fromdateParam = new SqlParameter("@FromDate", (object?)attendance.FromDate ?? DBNull.Value);
                var todateParam = new SqlParameter("@ToDate", (object?)attendance.ToDate ?? DBNull.Value);
                var statustypeParam = new SqlParameter("@Status", (object?)attendance.Status ?? DBNull.Value);
                var userlogin = new SqlParameter("@LoggedInUserId", (object?)attendance.LoggedInUserId ?? DBNull.Value);

                return await _db.Set<AttendanceRegularizationVM>()
              .FromSqlRaw("EXEC [dbo].[GetAttendanceRegularizationSearch] @SearchBy, @SearchValue, @FromDate,@ToDate, @Status,@LoggedInUserId",
                  searchbyParam, searchforParam, fromdateParam, todateParam, statustypeParam, userlogin)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceRegularizationVM>();
            }
        }

        public async Task<List<AttendanceRegularization>> SoftDelete(DeleteRecordVModel DeleteRecord)
        {
            var deletedRecords = new List<AttendanceRegularization>();

            foreach (var id in DeleteRecord.Id)
            {
                var attendance = await _db.AttendanceRegularization
                    .FirstOrDefaultAsync(x => x.AttendanceRegularizationId == id);

                if (attendance != null)
                {
                    attendance.IsEnabled = false;
                    attendance.IsDeleted = true;
                    attendance.DeletedDate = DateTime.UtcNow;
                    attendance.DeletedBy = DeleteRecord.DeletedBy;
                    deletedRecords.Add(attendance);

                }
            }

            return deletedRecords;
        }



        public async Task<bool> UpdateAttendanceRegularization(AttendanceRegularization updatedRecord)
        {

            var existingRecord = await _db.AttendanceRegularization
                .SingleOrDefaultAsync(x => x.AttendanceRegularizationId == updatedRecord.AttendanceRegularizationId);

            if (existingRecord == null)
            {
                return false; // Record not found
            }

            existingRecord.EmpId = updatedRecord.EmpId;
            existingRecord.FullName = updatedRecord.FullName;
            existingRecord.BranchName = updatedRecord.BranchName;
            existingRecord.ForDate = updatedRecord.ForDate;
            existingRecord.ShiftTime = updatedRecord.ShiftTime;
            existingRecord.InTime = updatedRecord.InTime;
            existingRecord.OutTime = updatedRecord.OutTime;
            existingRecord.Duration = updatedRecord.Duration;
            existingRecord.Status = updatedRecord.Status;
            existingRecord.Reason = updatedRecord.Reason;
            existingRecord.IsApproved = updatedRecord.IsApproved;
            existingRecord.IsPending = updatedRecord.IsPending;
            existingRecord.IsRejected = updatedRecord.IsRejected;
            existingRecord.UpdatedBy = updatedRecord.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }


        public async Task<bool> Update(AttendanceRegularization Record, int empInOutId)
        {
            var existingRecord = await _db.EmployeeInOutRecord.SingleOrDefaultAsync(asd => asd.Emp_IO_Id == empInOutId && asd.Emp_Id == Record.EmpId && asd.For_Date == Record.ForDate);
            if (existingRecord == null)
            {
                return false;
            }

            existingRecord.In_Time = Record.InTime;
            existingRecord.Out_Time = Record.OutTime;
            existingRecord.Duration = Record.Duration.ToString();
            existingRecord.Reason = Record.Reason;
            existingRecord.UpdatedBy = Record.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        public async Task<List<EmpInOutVM>> GetEmployeeInOut(int? EmpId, DateTime? ForDate)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@EmpId", EmpId),
                    new SqlParameter("@ForDate", ForDate),
                };

                var result = await _db.Set<EmpInOutVM>()
                    .FromSqlRaw("EXEC GetEmployeeInOut @EmpId, @ForDate ", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<EmpInOutVM>();
            }
        }

        public async Task<APIResponse> Create(AttendanceRegularization model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "INSERT");
                    parameters.Add("@EmpId", model.EmpId);
                    parameters.Add("@FullName", model.FullName);
                    parameters.Add("@BranchName", model.BranchName);
                    parameters.Add("@ForDate", model.ForDate);
                    parameters.Add("@ShiftTime", model.ShiftTime);
                    parameters.Add("@InTime", model.InTime);
                    parameters.Add("@OutTime", model.OutTime);
                    parameters.Add("@Day", model.Day);
                    parameters.Add("@Reason", model.Reason);
                    parameters.Add("@Remark", model.Remark);
                    parameters.Add("@Status", model.Status);
                    parameters.Add("@CreatedBy", model.CreatedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                    await connection.ExecuteAsync(
                        "SP_AttendanceRegularization",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = null;
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

        public async Task<APIResponse> Update(AttendanceRegularization model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "UPDATE");
                    parameters.Add("@Id", model.AttendanceRegularizationId);
                    parameters.Add("@EmpId", model.EmpId);
                    parameters.Add("@FullName", model.FullName);
                    parameters.Add("@BranchName", model.BranchName);
                    parameters.Add("@ForDate", model.ForDate);
                    parameters.Add("@ShiftTime", model.ShiftTime);
                    parameters.Add("@InTime", model.InTime);
                    parameters.Add("@OutTime", model.OutTime);
                    parameters.Add("@Day", model.Day);
                    parameters.Add("@Reason", model.Reason);
                    parameters.Add("@Remark", model.Remark);
                    parameters.Add("@Status", model.Status);
                    parameters.Add("@CreatedBy", model.UpdatedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                    await connection.ExecuteAsync(
                        "SP_AttendanceRegularization",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = null;
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

        public async Task<APIResponse> Delete(DeleteRecordVModel deleteRecord)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    foreach (int id in deleteRecord.Id)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@Action", "DELETE");
                        parameters.Add("@Id", id);
                        parameters.Add("@CreatedBy", deleteRecord.DeletedBy);
                        parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                        await connection.ExecuteAsync(
                            "SP_AttendanceRegularization",
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        response.isSuccess = parameters.Get<bool>("@Success");
                        response.ResponseMessage = parameters.Get<string>("@ResponseMessage");

                        // If any delete fails, stop processing
                        if (!response.isSuccess)
                            break;
                    }
                    response.Data = null;
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
        public async Task<List<AttendanceDetails>> GetAttendanceDetails(EmployeeInOutFilterVM outFilterVM)
        {
            try
            {
                var monthParam = new SqlParameter("@StartDate", (object?)outFilterVM.StartDate ?? DBNull.Value);
                var yearParam = new SqlParameter("@EndDate", (object?)outFilterVM.EndDate ?? DBNull.Value);
                var empCodeParam = new SqlParameter("@EmpId", (object?)outFilterVM.EmpId ?? DBNull.Value);

                return await _db.Set<AttendanceDetails>()
              .FromSqlRaw("EXEC SP_GetAttendanceDetails @StartDate,@EndDate,@EmpId",
                    monthParam, yearParam, empCodeParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceDetails>();
            }
        }

        public async Task<List<AttendanceRegularizationAdmin>> GetAttendanceRegularizationForAdmin(AttendanceRegularizationSearchFilterForAdminVM attendance)
        {
            try
            {
                var searchbyParam = new SqlParameter("@SearchBy", (object?)attendance.SearchBy ?? DBNull.Value);
                var searchforParam = new SqlParameter("@SearchValue", (object?)attendance.SearchValue ?? DBNull.Value);
                var fromdateParam = new SqlParameter("@FromDate", (object?)attendance.FromDate ?? DBNull.Value);
                var todateParam = new SqlParameter("@ToDate", (object?)attendance.ToDate ?? DBNull.Value);
                var statustypeParam = new SqlParameter("@Status", (object?)attendance.Status ?? DBNull.Value);
                var companyidParam = new SqlParameter("@CompanyId", (object?)attendance.CompanyId ?? DBNull.Value);
                var EmpidParam = new SqlParameter("@EmployeeId", (object?)attendance.EmployeeId ?? DBNull.Value);

                return await _db.Set<AttendanceRegularizationAdmin>()
              .FromSqlRaw("EXEC [dbo].[GetAttendanceRegularizationSearchForAdmin] @SearchBy, @SearchValue, @FromDate,@ToDate, @Status,@CompanyId,@EmployeeId",
                  searchbyParam, searchforParam, fromdateParam, todateParam, statustypeParam, companyidParam, EmpidParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceRegularizationAdmin>();
            }
        }

        public async Task<List<AttendanceRegularizationVM>> GetAttendanceRegularizationApproval(AttendanceRegularizationSearchFilterVM attendance)
        {
            try
            {
                var searchbyParam = new SqlParameter("@SearchBy", (object?)attendance.SearchBy ?? DBNull.Value);
                var searchforParam = new SqlParameter("@SearchValue", (object?)attendance.SearchValue ?? DBNull.Value);
                var fromdateParam = new SqlParameter("@FromDate", (object?)attendance.FromDate ?? DBNull.Value);
                var todateParam = new SqlParameter("@ToDate", (object?)attendance.ToDate ?? DBNull.Value);
                var statustypeParam = new SqlParameter("@Status", (object?)attendance.Status ?? DBNull.Value);
                var userlogin = new SqlParameter("@LoggedInUserId", (object?)attendance.LoggedInUserId ?? DBNull.Value);

                return await _db.Set<AttendanceRegularizationVM>()
              .FromSqlRaw("EXEC [dbo].[GetAttendanceRegularizationApproval] @SearchBy, @SearchValue, @FromDate,@ToDate, @Status,@LoggedInUserId",
                  searchbyParam, searchforParam, fromdateParam, todateParam, statustypeParam, userlogin)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceRegularizationVM>();
            }
        }

        public async Task<List<AttendanceRegularizationAdmin>> GetAttendanceRequestAdminReport(AttendanceRequestReportFilterVm attendance)
        {
            try
            {
                var searchbyParam = new SqlParameter("@SearchBy", (object?)attendance.SearchBy ?? DBNull.Value);
                var searchforParam = new SqlParameter("@SearchValue", (object?)attendance.SearchValue ?? DBNull.Value);
                var fromdateParam = new SqlParameter("@FromDate", (object?)attendance.FromDate ?? DBNull.Value);
                var todateParam = new SqlParameter("@ToDate", (object?)attendance.ToDate ?? DBNull.Value);
                var statustypeParam = new SqlParameter("@Status", (object?)attendance.Status ?? DBNull.Value);
                var compidParam = new SqlParameter("@CompanyId", (object?)attendance.CompanyId ?? DBNull.Value);

                return await _db.Set<AttendanceRegularizationAdmin>()
              .FromSqlRaw("EXEC [dbo].[GetAttendanceRequestAdminReport] @SearchBy, @SearchValue, @FromDate,@ToDate, @Status ,@CompanyId",
                  searchbyParam, searchforParam, fromdateParam, todateParam, statustypeParam, compidParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceRegularizationAdmin>();
            }
        }

        public async Task<List<EMpDetails>> GetEmployeeDetails(int? EmpId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@EmpId", EmpId),

                };

                var result = await _db.Set<EMpDetails>()
                    .FromSqlRaw("EXEC GetEmployeeAttendanceDetails @EmpId ", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<EMpDetails>();
            }
        }

        public async Task<List<AttendanceCount>> GetEmployeeAttendanceRequestsCountForCurrentMonth(int? EmpId, int Month, int Year)
        {
            try
            {
                var parameters = new[]
                {
           new SqlParameter("@EmpId", EmpId),
           new SqlParameter("@Month", Month),
           new SqlParameter("@Year", Year),

       };

                var result = await _db.Set<AttendanceCount>()
                    .FromSqlRaw("EXEC GetEmployeeAttendanceRequestsCountForCurrentMonth @EmpId ,@Month,@Year ", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceCount>();
            }
        }


        public async Task<List<LimitedReasonvm>> GetAttendanceReasonsByLimitType()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<LimitedReasonvm>(
                    "GetAttendanceReasonsByLimitType",
                    commandType: CommandType.StoredProcedure
                );
                return result.AsList();
            }
        }
    }
}

