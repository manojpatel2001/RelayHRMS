using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class ReportingManagerDetailsRepository : IReportingManagerDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public ReportingManagerDetailsRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateReportingManagerDetail(ReportingManagerDetails model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageReportingManagerDetails
                    @Action = {"CREATE"},
                    @EmployeeId = {model.EmployeeId},
                    @EffectedDate = {model.EffectedDate},
                    @ReportingManagerId = {model.ReportingManagerId},
                    @MethodName = {model.MethodName},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateReportingManagerDetail(ReportingManagerDetails model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageReportingManagerDetails
                    @Action = {"UPDATE"},
                    @ReportingManagerDetailsId = {model.ReportingManagerDetailsId},
                    @EmployeeId = {model.EmployeeId},
                    @EffectedDate = {model.EffectedDate},
                    @ReportingManagerId = {model.ReportingManagerId},
                    @MethodName = {model.MethodName},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteReportingManagerDetail(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageReportingManagerDetails
                    @Action = {"DELETE"},
                    @ReportingManagerDetailsId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<ReportingManagerDetails?> GetReportingManagerDetailById(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<ReportingManagerDetails>().FromSqlInterpolated($@"
                EXEC GetReportingManagerDetailById
                    @ReportingManagerDetailsId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result?.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ReportingManagerDetails>> GetAllReportingManagerDetails(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<ReportingManagerDetails>().FromSqlInterpolated($@"
                EXEC GetAllReportingManagerDetails
                    @EmployeeId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<ReportingManagerDetails>();
            }
        }
    }

}
