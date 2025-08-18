using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class LeaveApplicationRepository: Repository<LeaveApplication>, ILeaveApplicationRepository
    {

        private HRMSDbContext _db;

        public LeaveApplicationRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<VMLeaveApplicationSearchResult>> GetLeaveApplicationsAsync(SearchVmCompOff filter)
        {
            try
            {
                var parameters = new[]
                {
                        new SqlParameter("@LeaveType", (object?)filter.LeaveType ?? DBNull.Value),
                        new SqlParameter("@LeaveStatus", (object?)filter.Status ?? DBNull.Value),
                          new SqlParameter("@EmpId", (object?)filter.Emplooyeid ?? DBNull.Value),
                          new SqlParameter("@CompId", (object?)filter.CompId ?? DBNull.Value),
                          new SqlParameter("@SearchFor", (object?)filter.SearchFor ?? DBNull.Value),

                };

                var result = await _db.Set<VMLeaveApplicationSearchResult>()
                    .FromSqlRaw("EXEC SP_GetLeaveApplications @LeaveType, @LeaveStatus,@EmpId,@CompId,@SearchFor", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveApplicationsAsync Error: " + ex.Message);
                return new List<VMLeaveApplicationSearchResult>();
            }
        }

        public async Task<List<VmLeaveApplicationforApprove>> GetLeaveApplicationsforApprove(SearchVmCompOff filter)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@SearchType", (object?)filter.SearchType ?? DBNull.Value),
                    new SqlParameter("@SearchFor", (object?)filter.SearchFor ?? DBNull.Value),
                    new SqlParameter("@EmpId", (object?)filter.Emplooyeid ?? DBNull.Value),
                        new SqlParameter("@CompId", (object?)filter.CompId ?? DBNull.Value),
                };

                        var result = await _db.Set<VmLeaveApplicationforApprove>()
                            .FromSqlRaw("EXEC SP_GetLeaveApplicationsForAproval @SearchType, @SearchFor,@EmpId,@CompId", parameters)
                            .ToListAsync();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("GetLeaveApplicationsforApprove Error: " + ex.Message);
                        return new List<VmLeaveApplicationforApprove>();
                    }
        }

        public async Task<List<VmLeaveApplicationforApprove>> GetLeaveApplicationsforApproveAdmin(SearchVmCompOff filter)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@SearchType", (object?)filter.SearchType ?? DBNull.Value),
                    new SqlParameter("@SearchFor", (object?)filter.SearchFor ?? DBNull.Value),
                    new SqlParameter("@BranchId", (object?)filter.BranchId ?? DBNull.Value),
                    new SqlParameter("@CompId", (object?)filter.CompId ?? DBNull.Value),

                   
                };

                var result = await _db.Set<VmLeaveApplicationforApprove>()
                    .FromSqlRaw("EXEC SP_GetLeaveApplicationsForApprovalAdmin @SearchType, @SearchFor,@BranchId,@CompId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveApplicationsforApproveAdmin Error: " + ex.Message);
                return new List<VmLeaveApplicationforApprove>();
            }

        }

        public async Task<List<LeaveApprovalReportVM>> GetLeaveApproval(LeaveApp_Param vm)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@StartDate", (object?)vm.StartDate ?? DBNull.Value),
                    new SqlParameter("@EndDate", (object?)vm.EndDate ?? DBNull.Value),
                    new SqlParameter("@Status", (object?)vm.Status ?? DBNull.Value),
                    new SqlParameter("@EmpId", (object?)vm.EmpId ?? DBNull.Value),
                    new SqlParameter("@CompId", (object?)vm.CompId ?? DBNull.Value)

                };

                var result = await _db.Set<LeaveApprovalReportVM>()
                    .FromSqlRaw("EXEC SP_GetLeaveApproval @StartDate, @EndDate,@Status,@EmpId,@CompId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SP_GetLeaveApproval Error: " + ex.Message);
                return new List<LeaveApprovalReportVM>();
            }

        }

        public async Task<List<LeaveBalanceViewModel>> GetLeaveBalance(LeaveApp_Param vm)
        {
            try
            {
                var parameters = new[]
                {
                             
                    new SqlParameter("@CompId", (object?)vm.CompId ?? DBNull.Value),
                    new SqlParameter("@EmpId", (object?)vm.EmpId ?? DBNull.Value),
                     new SqlParameter("@StartDate", (object?)vm.StartDate ?? DBNull.Value),
                    new SqlParameter("@EndDate", (object?)vm.EndDate ?? DBNull.Value),
                      new SqlParameter("@LeaveType", (object?)vm.Status ?? DBNull.Value)
                };

                var result = await _db.Set<LeaveBalanceViewModel>()
                    .FromSqlRaw("EXEC GetLeaveBalance @CompId, @EmpId,@StartDate,@EndDate,@LeaveType", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveBalance Error: " + ex.Message);
                return new List<LeaveBalanceViewModel>();
            }

        }

        public async Task<List<LeaveTypevm>> GetLeaveDetails(LeaveDetailsvm vm)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@CompId", (object)vm.Compid ?? DBNull.Value),
            new SqlParameter("@EmpId", (object)vm.Empid ?? DBNull.Value)
        };

                var result = await _db.Set<LeaveTypevm>()
                    .FromSqlRaw("EXEC GetEmployeeLeaveDetails @CompId, @EmpId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveDetails Error: " + ex.Message);
                return new List<LeaveTypevm>();
            }
        }


        //public async Task<bool> InsertLeaveApplicationAsync(LeaveApplication model)
        //{
        //    try
        //    {
        //        var result = await _db.Database.ExecuteSqlRawAsync(
        //            "EXEC InsertLeaveApplication @EmplooyeId,@CompanyId, @ReportingManagerId, @LeaveType, @ApplicationType, @FromDate, @No_Of_Date, @Todate, @Reason, @Responsibleperson, @Cancel_Weekoff, @Send_Intimate,@LeaveStatus, @CreatedDate, @CreatedBy",
        //            new SqlParameter("@EmplooyeId", model.EmplooyeId ?? (object)DBNull.Value),
        //            new SqlParameter("@CompanyId", model.CompId ?? (object)DBNull.Value),
        //            new SqlParameter("@ReportingManagerId", model.ReportingManagerId ?? (object)DBNull.Value),
        //            new SqlParameter("@LeaveType", model.LeaveType ?? (object)DBNull.Value),
        //            new SqlParameter("@ApplicationType", model.ApplicationType ?? (object)DBNull.Value),
        //            new SqlParameter("@FromDate", model.FromDate ?? (object)DBNull.Value),
        //            new SqlParameter("@No_Of_Date", model.No_Of_Date ?? (object)DBNull.Value),
        //            new SqlParameter("@Todate", model.Todate ?? (object)DBNull.Value),
        //            new SqlParameter("@Reason", model.Reason ?? (object)DBNull.Value),
        //            new SqlParameter("@Responsibleperson", model.Responsibleperson ?? (object)DBNull.Value),
        //            new SqlParameter("@Cancel_Weekoff", model.Cancel_Weekoff ?? (object)DBNull.Value),
        //            new SqlParameter("@Send_Intimate", model.Send_Intimate ?? (object)DBNull.Value),
        //            new SqlParameter("@LeaveStatus", model.LeaveStatus ?? (object)DBNull.Value),
        //            new SqlParameter("@CreatedDate", model.CreatedDate),
        //            new SqlParameter("@CreatedBy", model.CreatedBy ?? (object)DBNull.Value)
        //        );

        //        return result > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("InsertLeaveApplicationAsync Error: " + ex.Message);
        //        return false;
        //    }
        //}

        public async Task<SP_Response> InsertLeaveApplicationAsync(LeaveApplication model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                EXEC InsertLeaveApplication
                    @EmployeeId = {model.EmplooyeId},
                    @CompanyId = {model.CompId},
                    @ReportingManagerId = {model.ReportingManagerId},
                    @LeaveType = {model.LeaveType},
                    @ApplicationType = {model.ApplicationType},
                    @FromDate = {model.FromDate},
                    @No_Of_Date = {model.No_Of_Date},
                    @Todate = {model.Todate},
                    @Reason = {model.Reason},
                    @Responsibleperson = {model.Responsibleperson},
                    @Cancel_Weekoff = {model.Cancel_Weekoff},
                    @Send_Intimate = {model.Send_Intimate},
                    @LeaveStatus = {model.LeaveStatus},
                    @Day = {model.Day},
                    @CreatedDate = {model.CreatedDate},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!"};
            }
        }

        public async Task<bool> softdelete(LeaveApplication Leave)
        {
            var leave = await _db.LeaveApplication.FirstOrDefaultAsync(asd => asd.FromDate == Leave.FromDate);
            if (leave == null)
            {
                return false;
            }
            else
            {
                leave.IsEnabled = false;
                leave.IsDeleted = true;
                leave.DeletedDate = DateTime.UtcNow;
                leave.DeletedBy = Leave.EmplooyeId?.ToString();

                return true;
            }
        }

        public async Task<bool> Updateapproval(List<int> applicationid, string status, DateTime Date)
        {
           
            try
            {
                foreach (var id in applicationid)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@ApplicationId", id),
                        new SqlParameter("@LeaveStatus", status),
                        new SqlParameter("@ApproveDate", Date)
                    };

                    await _db.Database.ExecuteSqlRawAsync(
                        "EXEC SP_LeaveApproveReject @ApplicationId, @LeaveStatus,@ApproveDate",
                        parameters
                    );
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during SP call: " + ex.Message);
                return false;
            }
        

        }

        public async Task<LeaveApplication?> GetLeaveApplicationById(int leaveApplicationId)
        {
            try
            {
                var result = await _db.Set<LeaveApplication>()
                    .FromSqlInterpolated($"EXEC GetLeaveApplicationById @LeaveApplicationId = {leaveApplicationId}")
                    .ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ActiveLeaveDetailsvm>> GetActiveLeaveDetails()
        {
            try
            {
              
                var result = await _db.Set<ActiveLeaveDetailsvm>()
                    .FromSqlRaw("EXEC GetActiveLeaveDetails" )
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetActiveLeaveDetails Error: " + ex.Message);
                return new List<ActiveLeaveDetailsvm>();
            }

        }
    }
}
