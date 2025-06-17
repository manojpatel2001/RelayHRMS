using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Core.VM.PrivilegeSetting;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.PrivilegeSetting
{
    public class PageMasterRepository:Repository<PageMaster>, IPageMasterRepository
    {
        private readonly HRMSDbContext _db;

        public PageMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreatePageMaster(PageMaster pageMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManagePageMaster
                        @Action = {"CREATE"},
                        @PageName = {pageMaster.PageName},
                        @AliasPageName = {pageMaster.AliasPageName},
                        @UnderPageMasterId = {pageMaster.UnderPageMasterId},
                        @UnderPageMasterName = {pageMaster.UnderPageMasterName},
                        @PageUrl = {pageMaster.PageUrl},
                        @SortId = {pageMaster.SortId},
                        @ModuleDetailsId = {pageMaster.ModuleDetailsId},
                        @PagePanelId = {pageMaster.PagePanelId},
                        @IsActive = {pageMaster.IsActive},
                        @IsDeleted = {pageMaster.IsDeleted},
                        @IsEnabled = {pageMaster.IsEnabled},
                        @IsBlocked = {pageMaster.IsBlocked},
                        @CreatedDate = {pageMaster.CreatedDate},
                        @CreatedBy = {pageMaster.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> UpdatePageMaster(PageMaster pageMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManagePageMaster
                        @Action = {"UPDATE"},
                        @PageMasterId = {pageMaster.PageMasterId},
                        @PageName = {pageMaster.PageName},
                        @AliasPageName = {pageMaster.AliasPageName},
                        @UnderPageMasterId = {pageMaster.UnderPageMasterId},
                        @UnderPageMasterName = {pageMaster.UnderPageMasterName},
                        @PagePanelId = {pageMaster.PagePanelId},
                        @PageUrl = {pageMaster.PageUrl},
                        @SortId = {pageMaster.SortId},
                        @ModuleDetailsId = {pageMaster.ModuleDetailsId},
                        @IsActive = {pageMaster.IsActive},
                        @UpdatedDate = {DateTime.UtcNow},
                        @UpdatedBy = {pageMaster.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> DeletePageMaster(DeleteRecordVM pageMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManagePageMaster
                        @Action = {"DELETE"},
                        @PageMasterId = {pageMaster.Id},
                        @DeletedDate = {DateTime.UtcNow},
                        @DeletedBy = {pageMaster.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<PageMaster?> GetPageMasterById(int pageMasterId)
        {
            try
            {
                var result = await _db.Set<PageMaster>()
                                      .FromSqlInterpolated($"EXEC GetPageMasterById @PageMasterId = {pageMasterId}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<vmPageMaster>> GetAllPageMaster()
        {
            try
            {
                var result = await _db.Set<vmPageMaster>()
                                      .FromSqlInterpolated($"EXEC GetAllPageMaster")
                                      .ToListAsync();

                return result ?? new List<vmPageMaster>();
            }
            catch (Exception ex)
            {
                return new List<vmPageMaster>();
            }
        }
        public async Task<List<PageMaster>> GetAllMenuPages()
        {
            try
            {
                var result = await _db.Set<PageMaster>()
                                      .FromSqlInterpolated($"EXEC GetAllMenuPages")
                                      .ToListAsync();

                return result ?? new List<PageMaster>();
            }
            catch (Exception ex)
            {
                return new List<PageMaster>();
            }
        }

        public async Task<List<vmPageMaster>> GetAllPagesByPagePanel(int pagePanelId)
        {
            try
            {
                var result = await _db.Set<vmPageMaster>()
                                      .FromSqlInterpolated($"EXEC GetAllPagesByPagePanel @PagePanelId={pagePanelId}")
                                      .ToListAsync();

                return result ?? new List<vmPageMaster>();
            }
            catch (Exception ex)
            {
                return new List<vmPageMaster>();
            }
        }

        public async  Task<List<PanelHierarchyVM>> GetPageHierarchyWithPrivileges(PageVM pageVM)
        {
            try
            {
                using (var conn = _db.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "GetPageHierarchyWithPrivileges";
                        cmd.CommandType = CommandType.StoredProcedure;

                        var companyParam = cmd.CreateParameter();
                        companyParam.ParameterName = "@CompanyId";
                        companyParam.Value = pageVM.CompanyId;
                        cmd.Parameters.Add(companyParam);

                        var panelParam = cmd.CreateParameter();
                        panelParam.ParameterName = "@PagePanelId";
                        panelParam.Value = pageVM.PagePanelId;
                        cmd.Parameters.Add(panelParam);

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
