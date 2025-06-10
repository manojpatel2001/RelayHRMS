using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using HRMS_Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EmployeeContactRepository : Repository<EmployeeContact>, IEmployeeContactRepository
{
    private readonly HRMSDbContext _db;

    public EmployeeContactRepository(HRMSDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<VMCommonResult> CreateEmployeeContact(EmployeeContact contact)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageEmployeeContact
                    @Action = {"CREATE"},
                    @PresentAddress = {contact.PresentAddress},
                    @PresentTehsil = {contact.PresentTehsil},
                    @PresentDistrict = {contact.PresentDistrict},
                    @PresentCity = {contact.PresentCity},
                    @PresentState = {contact.PresentState},
                    @PresentPincode = {contact.PresentPincode},
                    @PresentThanaId = {contact.PresentThanaId},
                    @PermanentAddress = {contact.PermanentAddress},
                    @PermanentTehsil = {contact.PermanentTehsil},
                    @PermanentDistrict = {contact.PermanentDistrict},
                    @PermanentCity = {contact.PermanentCity},
                    @PermanentState = {contact.PermanentState},
                    @PermanentPincode = {contact.PermanentPincode},
                    @PermanentThanaId = {contact.PermanentThanaId},
                    @CountryId = {contact.CountryId},
                    @WorkPhone = {contact.WorkPhone},
                    @PersonalPhone = {contact.PersonalPhone},
                    @OfficialEmail = {contact.OfficialEmail},
                    @Nationality = {contact.Nationality},
                    @ExtensionNo = {contact.ExtensionNo},
                    @MobileNo = {contact.MobileNo},
                    @SameAsPresentAddress = {contact.SameAsPresentAddress},
                    @EmployeeId = {contact.EmployeeId},
                    @IsDeleted = {contact.IsDeleted},
                    @IsEnabled = {contact.IsEnabled},
                    @IsBlocked = {contact.IsBlocked},
                    @CreatedDate = {contact.CreatedDate},
                    @CreatedBy = {contact.CreatedBy}
            ").ToListAsync();

            return result.FirstOrDefault() ?? new VMCommonResult { Id = null};
        }
        catch (Exception ex)
        {
            return new VMCommonResult { Id = null };
        }
    }

    public async Task<VMCommonResult> UpdateEmployeeContact(EmployeeContact contact)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageEmployeeContact
                    @Action = {"UPDATE"},
                    @EmployeeContactId = {contact.EmployeeContactId},
                    @PresentAddress = {contact.PresentAddress},
                    @PresentTehsil = {contact.PresentTehsil},
                    @PresentDistrict = {contact.PresentDistrict},
                    @PresentCity = {contact.PresentCity},
                    @PresentState = {contact.PresentState},
                    @PresentPincode = {contact.PresentPincode},
                    @PresentThanaId = {contact.PresentThanaId},
                    @PermanentAddress = {contact.PermanentAddress},
                    @PermanentTehsil = {contact.PermanentTehsil},
                    @PermanentDistrict = {contact.PermanentDistrict},
                    @PermanentCity = {contact.PermanentCity},
                    @PermanentState = {contact.PermanentState},
                    @PermanentPincode = {contact.PermanentPincode},
                    @PermanentThanaId = {contact.PermanentThanaId},
                    @CountryId = {contact.CountryId},
                    @WorkPhone = {contact.WorkPhone},
                    @PersonalPhone = {contact.PersonalPhone},
                    @OfficialEmail = {contact.OfficialEmail},
                    @Nationality = {contact.Nationality},
                    @ExtensionNo = {contact.ExtensionNo},
                    @MobileNo = {contact.MobileNo},
                    @SameAsPresentAddress = {contact.SameAsPresentAddress},
                    @EmployeeId = {contact.EmployeeId},
                    @UpdatedDate = {DateTime.UtcNow},
                    @UpdatedBy = {contact.UpdatedBy}
            ").ToListAsync();

            return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
        }
        catch (Exception ex)
        {
            return new VMCommonResult { Id = null};
        }
    }

    public async Task<VMCommonResult> DeleteEmployeeContact(DeleteRecordVM contact)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageEmployeeContact
                    @Action = {"DELETE"},
                    @EmployeeContactId = {contact.Id},
                    @DeletedDate = {DateTime.UtcNow},
                    @DeletedBy = {contact.DeletedBy}
            ").ToListAsync();

            return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
        }
        catch (Exception ex)
        {
            return new VMCommonResult { Id = null};
        }
    }

    
    public async Task<EmployeeContact?> GetEmployeeContactById(int employeeContactId)
    {
        try
        {
            var result = await _db.Set<EmployeeContact>()
                                  .FromSqlInterpolated($"EXEC GetEmployeeContactById @EmployeeContactId = {employeeContactId}")
                                  .ToListAsync();

            return result.FirstOrDefault()??null;
        }
        catch (Exception ex)
        {
            // Log exception here if needed
            return null;
        }
    }



    public async Task<EmployeeContact?> GetEmployeeContactByEmployeeId(string employeeId)
    {
        try
        {
            var result = await _db.Set<EmployeeContact>()
                                  .FromSqlInterpolated($"EXEC GetEmployeeContactByEmployeeId @EmployeeId = {employeeId}")
                                  .ToListAsync();

            return result.FirstOrDefault() ?? null;
        }
        catch (Exception ex)
        {
            // Log exception here if needed
            return null;
        }
    }
}
