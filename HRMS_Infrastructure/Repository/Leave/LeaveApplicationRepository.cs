using HRMS_Core.DbContext;
using HRMS_Core.Leave;
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
            new SqlParameter("@LeaveType", (object?)filter.SearchType ?? DBNull.Value),
            new SqlParameter("@LeaveStatus", (object?)filter.Status ?? DBNull.Value),
        };

                var result = await _db.Set<VMLeaveApplicationSearchResult>()
                    .FromSqlRaw("EXEC SP_GetLeaveApplications @LeaveType, @LeaveStatus", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveApplicationsAsync Error: " + ex.Message);
                return new List<VMLeaveApplicationSearchResult>();
            }
        }



        public async Task<bool> InsertLeaveApplicationAsync(LeaveApplication model)
        {
            try
            {
                var result = await _db.Database.ExecuteSqlRawAsync(
                    "EXEC InsertLeaveApplication @EmplooyeId, @ReportingManagerId, @LeaveType, @ApplicationType, @FromDate, @No_Of_Date, @Todate, @Reason, @Responsibleperson, @Cancel_Weekoff, @Send_Intimate,@LeaveStatus, @CreatedDate, @CreatedBy",
                    new SqlParameter("@EmplooyeId", model.EmplooyeId ?? (object)DBNull.Value),
                    new SqlParameter("@ReportingManagerId", model.ReportingManagerId ?? (object)DBNull.Value),
                    new SqlParameter("@LeaveType", model.LeaveType ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicationType", model.ApplicationType ?? (object)DBNull.Value),
                    new SqlParameter("@FromDate", model.FromDate ?? (object)DBNull.Value),
                    new SqlParameter("@No_Of_Date", model.No_Of_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Todate", model.Todate ?? (object)DBNull.Value),
                    new SqlParameter("@Reason", model.Reason ?? (object)DBNull.Value),
                    new SqlParameter("@Responsibleperson", model.Responsibleperson ?? (object)DBNull.Value),
                    new SqlParameter("@Cancel_Weekoff", model.Cancel_Weekoff ?? (object)DBNull.Value),
                    new SqlParameter("@Send_Intimate", model.Send_Intimate ?? (object)DBNull.Value),
                    new SqlParameter("@LeaveStatus", model.LeaveStatus ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedDate", model.CreatedDate),
                    new SqlParameter("@CreatedBy", model.CreatedBy ?? (object)DBNull.Value)
                );

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("InsertLeaveApplicationAsync Error: " + ex.Message);
                return false;
            }
        }

    }
}
