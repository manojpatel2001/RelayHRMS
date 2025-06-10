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

public class CountryRepository : Repository<Country>, ICountryRepository
{
    private readonly HRMSDbContext _db;

    public CountryRepository(HRMSDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<List<Country>> GetAllCountries()
    {
        try
        {
            return await _db.Set<Country>().FromSqlInterpolated($"EXEC GetAllCountries").ToListAsync();
        }
        catch
        {
            return new List<Country>();
        }
    }

    public async Task<Country?> GetCountryById(int countryId)
    {
        try
        {
            var result = await _db.Set<Country>()
                .FromSqlInterpolated($"EXEC GetCountryById @CountryId = {countryId}").ToListAsync();

            return result.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    public async Task<VMCommonResult> CreateCountry(Country country)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageCountry
                    @Action = {"CREATE"},
                    @CountryName = {country.CountryName},
                    @IsDeleted = {country.IsDeleted},
                    @IsEnabled = {country.IsEnabled},
                    @IsBlocked = {country.IsBlocked},
                    @CreatedDate = {country.CreatedDate},
                    @CreatedBy = {country.CreatedBy}
            ").ToListAsync();

            return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
        }
        catch
        {
            return new VMCommonResult { Id = 0 };
        }
    }

    public async Task<VMCommonResult> UpdateCountry(Country country)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageCountry
                    @Action = {"UPDATE"},
                    @CountryId = {country.CountryId},
                    @CountryName = {country.CountryName},
                    @UpdatedDate = {country.UpdatedDate},
                    @UpdatedBy = {country.UpdatedBy}
            ").ToListAsync();

            return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
        }
        catch
        {
            return new VMCommonResult { Id = 0 };
        }
    }

    public async Task<VMCommonResult> DeleteCountry(DeleteRecordVM deleteRecord)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageCountry
                    @Action = {"DELETE"},
                    @CountryId = {deleteRecord.Id},
                    @DeletedDate = {deleteRecord.DeletedDate},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

            return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
        }
        catch
        {
            return new VMCommonResult { Id = 0 };
        }
    }
}
