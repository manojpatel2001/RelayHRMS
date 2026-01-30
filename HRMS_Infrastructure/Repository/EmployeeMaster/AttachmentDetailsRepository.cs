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

            public async Task<SP_Response> CreateAttachmentDetail(VmAttachmentDetails model)
            {
                try
                {
                    var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageAttachmentDetails
                        @Action = {"CREATE"},
                        @EmployeeId = {model.EmployeeId},
                        @DocumentTypeId = {model.DocumentTypeId},
                        @DocumentUrl = {model.DocumentUrl},
                        @Comment = {model.Comment},
                        @DateOfExpiry = {model.DateOfExpiry},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                    return result.FirstOrDefault()?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
                catch
                {
                    return new SP_Response { Success = 0 ,ResponseMessage="Something went wrong!"};
                }
            }

            public async Task<SP_Response> UpdateAttachmentDetail(VmAttachmentDetails model)
            {
                try
                {
                    var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageAttachmentDetails
                        @Action = {"UPDATE"},
                        @AttachmentDetailsId = {model.AttachmentDetailsId},
                        @EmployeeId = {model.EmployeeId},
                        @DocumentTypeId = {model.DocumentTypeId},
                        @DocumentUrl = {model.DocumentUrl},
                        @Comment = {model.Comment},
                        @DateOfExpiry = {model.DateOfExpiry},
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                   return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
                catch
                {
                    return new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
            }

            public async Task<SP_Response> DeleteAttachmentDetail(DeleteRecordVM deleteRecord)
            {
                try
                {
                    var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageAttachmentDetails
                        @Action = {"DELETE"},
                        @AttachmentDetailsId = {deleteRecord.Id},
                        @DeletedBy = {deleteRecord.DeletedBy}
                 ").ToListAsync();

                  return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
                catch
                {
                    return new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
        }

            public async Task<AttachmentDetails?> GetAttachmentDetailById(vmCommonGetById vmCommonGetById)
            {
                try
                {
                    var result = await _db.Set<AttachmentDetails>().FromSqlInterpolated($@"
                    EXEC GetAttachmentDetailById
                        @AttachmentDetailsId = {vmCommonGetById.Id}
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
                        @EmployeeId = {vmCommonGetById.Id}
                        
                ").ToListAsync();

                    return result;
                }
                catch
                {
                    return new List<AttachmentDetails>();
                }
            }
        
            public async Task<List<DocumentType>> GetAllDocumentTypes()
            {
                try
                {
                    var result = await _db.Set<DocumentType>().FromSqlInterpolated($@"
                    EXEC GetAllDocumentTypes
                        
                ").ToListAsync();

                    return result;
                }
                catch
                {
                    return new List<DocumentType>();
                }
            }
        
    }
}
