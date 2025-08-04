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
    public class BusinessSegmentRepository:IBusinessSegmentRepository
    {
        private readonly HRMSDbContext _db;

        public BusinessSegmentRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateBusinessDetail(BusinessSegment model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageBusinessDetail
                        @Action = {"CREATE"},
                        @BusinessSegmentName = {model.BusinessSegmentName},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateBusinessDetail(BusinessSegment model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageBusinessDetail
                        @Action = {"UPDATE"},
                        @BusinessSegmentId = {model.BusinessSegmentId},
                        @BusinessName = {model.BusinessSegmentName},
                       
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteBusinessDetail(DeleteRecordVM model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageBusinessDetail
                        @Action = {"DELETE"},
                        @BusinessSegmentId = {model.Id},
                        @DeletedBy = {model.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<BusinessSegment?> GetBusinessDetailById(vmCommonGetById model)
        {
            try
            {
                var result = await _db.Set<BusinessSegment>().FromSqlInterpolated($@"
                    EXEC GetBusinessDetailById
                        @BusinessDetailId = {model.Id},
                        @IsDeleted = {model.IsDeleted},
                        @IsEnabled = {model.IsEnabled}
                ").ToListAsync();

                return result?.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<BusinessSegment>> GetAllBusinessSegments(vmCommonGetById model)
        {
            try
            {
                var result = await _db.Set<BusinessSegment>().FromSqlInterpolated($@"
                    EXEC GetAllBusinessSegments
                        @BusinessSegmentName = {model.Title},
                        @IsDeleted = {model.IsDeleted},
                        @IsEnabled = {model.IsEnabled}
                ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<BusinessSegment>();
            }
        }
    }
    
}
