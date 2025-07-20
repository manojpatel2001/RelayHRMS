using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.EmegencyContacts;
using Microsoft.EntityFrameworkCore;


namespace HRMS_Infrastructure.Repository.EmergencyContacts
{
    public class EmergencyContactRepository: IEmergencyContactRepository
    {
        private readonly HRMSDbContext _db;

        public EmergencyContactRepository(HRMSDbContext db) 
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateEmergencyContact(EmergencyContact model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageEmergencyContact
                    @Action = {"CREATE"},
                    @EmployeeId = {model.EmployeeId},
                    @Name = {model.Name},
                    @RelationShipId = {model.RelationShipId},
                    @MobileNo = {model.MobileNo},
                    @HomePhoneNo = {model.HomePhoneNo},
                    @WorkPhoneNo = {model.WorkPhoneNo},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateEmergencyContact(EmergencyContact model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageEmergencyContact
                    @Action = {"UPDATE"},
                    @EmergencyContactId = {model.EmergencyContactId},
                    @EmployeeId = {model.EmployeeId},
                    @Name = {model.Name},
                    @RelationShipId = {model.RelationShipId},
                    @MobileNo = {model.MobileNo},
                    @HomePhoneNo = {model.HomePhoneNo},
                    @WorkPhoneNo = {model.WorkPhoneNo},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteEmergencyContact(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageEmergencyContact
                    @Action = {"DELETE"},
                    @EmergencyContactId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<EmergencyContact?> GetEmergencyContactById(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<EmergencyContact>().FromSqlInterpolated($@"
                EXEC GetEmergencyContactById
                    @EmergencyContactId = {vmCommonGetById.Id},
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

        public async Task<List<EmergencyContact>> GetAllEmergencyContacts(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<EmergencyContact>().FromSqlInterpolated($@"
                EXEC GetAllEmergencyContacts
                  @EmployeeId = {vmCommonGetById.Id},
                  @IsDeleted = {vmCommonGetById.IsDeleted},
                  @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<EmergencyContact>();
            }
        }

        
    }
}
