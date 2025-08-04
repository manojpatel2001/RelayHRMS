using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
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
    public  class AttachmentDetailsRepository: IAttachmentDetailsRepository
    {
        private readonly HRMSDbContext _db;
        public AttachmentDetailsRepository(HRMSDbContext db)
            {
                _db = db;
            }

            public async Task<VMCommonResult> CreateAttachmentDetail(VmAttachmentDetails model)
            {
                try
                {
                    var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageAttachmentDetails
                        @Action = {"CREATE"},
                        @EmployeeId = {model.EmployeeId},
                        @DocumentName = {model.DocumentName},
                        @DocumentUrl = {model.DocumentUrl},
                        @Comment = {model.Comment},
                        @DateOfExpiry = {model.DateOfExpiry},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                    return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
                }
                catch
                {
                    return new VMCommonResult { Id = 0 };
                }
            }

            public async Task<VMCommonResult> UpdateAttachmentDetail(VmAttachmentDetails model)
            {
                try
                {
                    var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageAttachmentDetails
                        @Action = {"UPDATE"},
                        @AttachmentDetailsId = {model.AttachmentDetailsId},
                        @EmployeeId = {model.EmployeeId},
                        @DocumentName = {model.DocumentName},
                        @DocumentUrl = {model.DocumentUrl},
                        @Comment = {model.Comment},
                        @DateOfExpiry = {model.DateOfExpiry},
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                    return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
                }
                catch
                {
                    return new VMCommonResult { Id = 0 };
                }
            }

            public async Task<VMCommonResult> DeleteAttachmentDetail(DeleteRecordVM deleteRecord)
            {
                try
                {
                    var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageAttachmentDetails
                        @Action = {"DELETE"},
                        @AttachmentDetailsId = {deleteRecord.Id},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();

                    return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
                }
                catch
                {
                    return new VMCommonResult { Id = 0 };
                }
            }

            public async Task<AttachmentDetails?> GetAttachmentDetailById(vmCommonGetById vmCommonGetById)
            {
                try
                {
                    var result = await _db.Set<AttachmentDetails>().FromSqlInterpolated($@"
                    EXEC GetAttachmentDetailById
                        @AttachmentDetailsId = {vmCommonGetById.Id},
                        @IsDeleted = {vmCommonGetById.IsDeleted},
                        @IsEnabled = {vmCommonGetById.IsEnabled}
                ").ToListAsync();

                    return result?.FirstOrDefault() ?? null;
                }
                catch
                {
                    return null;
                }
            }

            public async Task<List<AttachmentDetails>> GetAllAttachmentDetails(vmCommonGetById vmCommonGetById)
            {
                try
                {
                    var result = await _db.Set<AttachmentDetails>().FromSqlInterpolated($@"
                    EXEC GetAllAttachmentDetails
                        @EmployeeId = {vmCommonGetById.Id},
                        @IsDeleted = {vmCommonGetById.IsDeleted},
                        @IsEnabled = {vmCommonGetById.IsEnabled}
                ").ToListAsync();

                    return result;
                }
                catch
                {
                    return new List<AttachmentDetails>();
                }
            }
        
    }
}
