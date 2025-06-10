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

public class ThanaRepository : Repository<Thana>, IThanaRepository
{
    private readonly HRMSDbContext _db;

    public ThanaRepository(HRMSDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<List<Thana>> GetAllThanas()
    {
        try
        {
            return await _db.Set<Thana>().FromSqlInterpolated($"EXEC GetAllThanas").ToListAsync();
        }
        catch
        {
            return new List<Thana>();
        }
    }

    public async Task<Thana?> GetThanaById(int thanaId)
    {
        try
        {
            var result = await _db.Set<Thana>()
                .FromSqlInterpolated($"EXEC GetThanaById @ThanaId = {thanaId}").ToListAsync();

            return result.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    public async Task<VMCommonResult> CreateThana(Thana thana)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageThana
                    @Action = {"CREATE"},
                    @ThanaName = {thana.ThanaName},
                    @IsDeleted = {thana.IsDeleted},
                    @IsEnabled = {thana.IsEnabled},
                    @IsBlocked = {thana.IsBlocked},
                    @CreatedDate = {thana.CreatedDate},
                    @CreatedBy = {thana.CreatedBy}
            ").ToListAsync();

            return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
        }
        catch
        {
            return new VMCommonResult { Id = 0 };
        }
    }

    public async Task<VMCommonResult> UpdateThana(Thana thana)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageThana
                    @Action = {"UPDATE"},
                    @ThanaId = {thana.ThanaId},
                    @ThanaName = {thana.ThanaName},
                    @UpdatedDate = {thana.UpdatedDate},
                    @UpdatedBy = {thana.UpdatedBy}
            ").ToListAsync();

            return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
        }
        catch
        {
            return new VMCommonResult { Id = 0 };
        }
    }

    public async Task<VMCommonResult> DeleteThana(DeleteRecordVM deleteRecord)
    {
        try
        {
            var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageThana
                    @Action = {"DELETE"},
                    @ThanaId = {deleteRecord.Id},
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
