using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Core.VM.PrivilegeSetting;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

    }
}
