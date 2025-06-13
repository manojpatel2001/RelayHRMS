using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.PrivilegeSetting
{
    public class ModuleDetailsRepository : Repository<ModuleDetails>, IModuleDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public ModuleDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateModuleDetails(ModuleDetails moduleDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageModuleDetails
                    @Action = {"CREATE"},
                    @ModuleName = {moduleDetails.ModuleName},
                    @Description = {moduleDetails.Description},
                    @CompanyId = {moduleDetails.CompanyId},
                    @CreatedDate = {moduleDetails.CreatedDate},
                    @CreatedBy = {moduleDetails.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> UpdateModuleDetails(ModuleDetails moduleDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageModuleDetails
                    @Action = {"UPDATE"},
                    @ModuleDetailsId = {moduleDetails.ModuleDetailsId},
                    @ModuleName = {moduleDetails.ModuleName},
                    @Description = {moduleDetails.Description},
                    @CompanyId = {moduleDetails.CompanyId},
                    @UpdatedDate = {DateTime.UtcNow},
                    @UpdatedBy = {moduleDetails.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> DeleteModuleDetails(DeleteRecordVM moduleDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageModuleDetails
                    @Action = {"DELETE"},
                    @ModuleDetailsId = {moduleDetails.Id},
                    @DeletedDate = {DateTime.UtcNow},
                    @DeletedBy = {moduleDetails.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<ModuleDetails?> GetModuleDetailsById(int moduleDetailsId)
        {
            try
            {
                var result = await _db.Set<ModuleDetails>()
                                      .FromSqlInterpolated($"EXEC GetModuleDetailsById @ModuleDetailsId = {moduleDetailsId}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ModuleDetails>> GetAllModuleDetailsByCompanyId(int companyId)
        {
            try
            {
                var result = await _db.Set<ModuleDetails>()
                                      .FromSqlInterpolated($"EXEC GetAllModuleDetailsByCompanyId  @CompanyId = {companyId}")
                                      .ToListAsync();

                return result ?? new List<ModuleDetails>();
            }
            catch (Exception ex)
            {
                return new List<ModuleDetails>();
            }
        }
    }

}
