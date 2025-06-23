using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Core.VM.PrivilegeSetting;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.PrivilegeSetting
{
    public class PrivilegeMasterRepository : Repository<PrivilegeMaster>, IPrivilegeMasterRepository
    {
        private readonly HRMSDbContext _db;

        public PrivilegeMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreatePrivilegeMaster(PrivilegeMaster privilegeMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [dbo].[ManagePrivilegeMaster]
                @Action = {"CREATE"},
                @PrivilegeName = {privilegeMaster.PrivilegeName},
                @CompanyId = {privilegeMaster.CompanyId},
                @BranchId_Multi = {privilegeMaster.BranchId_Multi},
                @DepartmentId_Multi = {privilegeMaster.DepartmentId_Multi},
                @VerticalId_Multi = {privilegeMaster.VerticalId_Multi},
                @PrivilegeType = {privilegeMaster.PrivilegeType},
                
                @CreatedDate = {privilegeMaster.CreatedDate},
                @CreatedBy = {privilegeMaster.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdatePrivilegeMaster(PrivilegeMaster privilegeMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [dbo].[ManagePrivilegeMaster]
                @Action = {"UPDATE"},
                @PrivilegeMasterId = {privilegeMaster.PrivilegeMasterId},
                @PrivilegeMasterId = {privilegeMaster.PrivilegeMasterId},
                @PrivilegeName = {privilegeMaster.PrivilegeName},
                @CompanyId = {privilegeMaster.CompanyId},
                @BranchId_Multi = {privilegeMaster.BranchId_Multi},
                @DepartmentId_Multi = {privilegeMaster.DepartmentId_Multi},
                @VerticalId_Multi = {privilegeMaster.VerticalId_Multi},
                @PrivilegeType = {privilegeMaster.PrivilegeType},
                @UpdatedDate = {DateTime.UtcNow},
                @UpdatedBy = {privilegeMaster.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeletePrivilegeMaster(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [dbo].[ManagePrivilegeMaster]
                @Action = {"DELETE"},
                @PrivilegeMasterId = {deleteRecord.Id},
                @DeletedDate = {DateTime.UtcNow},
                @DeletedBy = {deleteRecord.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<PrivilegeMaster?> GetPrivilegeMasterById(int privilegeMasterId)
        {
            try
            {
                var result = await _db.Set<PrivilegeMaster>()
                                      .FromSqlInterpolated($"EXEC GetPrivilegeMasterById @PrivilegeMasterId = {privilegeMasterId}")
                                      .ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return null;
            }
        }

        public async Task<List<vmGetAllPrivilegeMasterByCompanyId>> GetAllPrivilegeMasterByCompanyId(int companyId)
        {
            try
            {
                var result = await _db.Set<vmGetAllPrivilegeMasterByCompanyId>()
                                      .FromSqlInterpolated($"EXEC GetAllPrivilegeMasterByCompanyId @CompanyId = {companyId}")
                                      .ToListAsync();
                return result ?? new List<vmGetAllPrivilegeMasterByCompanyId>();
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new List<vmGetAllPrivilegeMasterByCompanyId>();
            }
        }

        public async Task<List<PanelHierarchyVM>> GetAllPageHierarchyByPrivilegeMasterId(PageVM pageVM)
        {
            try
            {
                using (var conn = _db.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand()) // ✅ Corrected this line
                    {
                        cmd.CommandText = "GetAllPageHierarchyByPrivilegeMasterId";
                        cmd.CommandType = CommandType.StoredProcedure;

                        var companyParam1 = cmd.CreateParameter();
                        companyParam1.ParameterName = "@CompanyId";
                        companyParam1.Value = pageVM.CompanyId;
                        cmd.Parameters.Add(companyParam1);

                        var panelParam1 = cmd.CreateParameter();
                        panelParam1.ParameterName = "@PagePanelId";
                        panelParam1.Value = pageVM.PagePanelId;
                        cmd.Parameters.Add(panelParam1);

                        var privilegeMasterId = cmd.CreateParameter();
                        privilegeMasterId.ParameterName = "@PrivilegeMasterId"; // ✅ Correct name
                        privilegeMasterId.Value = pageVM.PrivilegeMasterId;
                        cmd.Parameters.Add(privilegeMasterId); // ✅ Add the right object

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var json = reader.GetString(0);
                                return JsonSerializer.Deserialize<List<PanelHierarchyVM>>(json);
                            }
                        }
                    }
                }
                return new List<PanelHierarchyVM>();
            }
            catch (Exception ex)
            {
                return new List<PanelHierarchyVM>();
            }
        }
    }
}