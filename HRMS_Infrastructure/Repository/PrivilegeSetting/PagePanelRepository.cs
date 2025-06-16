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
    public class PagePanelRepository : Repository<PagePanel>, IPagePanelRepository
    {
        private readonly HRMSDbContext _db;

        public PagePanelRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreatePagePanel(PagePanel pagePanel)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePagePanel
                    @Action = {"CREATE"},
                    @PagePanelName = {pagePanel.PagePanelName},
                    @AliasPanel = {pagePanel.AliasPanel},
                    @IsDeleted = {pagePanel.IsDeleted},
                    @IsEnabled = {pagePanel.IsEnabled},
                    @IsBlocked = {pagePanel.IsBlocked},
                    @CreatedDate = {pagePanel.CreatedDate},
                    @CreatedBy = {pagePanel.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> UpdatePagePanel(PagePanel pagePanel)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePagePanel
                    @Action = {"UPDATE"},
                    @PagePanelId = {pagePanel.PagePanelId},
                    @PagePanelName = {pagePanel.PagePanelName},
                    @AliasPanel = {pagePanel.AliasPanel},
                    @UpdatedDate = {DateTime.UtcNow},
                    @UpdatedBy = {pagePanel.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> DeletePagePanel(DeleteRecordVM pagePanel)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePagePanel
                    @Action = {"DELETE"},
                    @PagePanelId = {pagePanel.Id},
                    @DeletedDate = {DateTime.UtcNow},
                    @DeletedBy = {pagePanel.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<PagePanel?> GetPagePanelById(int pagePanelId)
        {
            try
            {
                var result = await _db.Set<PagePanel>()
                                      .FromSqlInterpolated($"EXEC GetPagePanelById @PagePanelId = {pagePanelId}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<PagePanel>> GetAllPagePanels()
        {
            try
            {
                var result = await _db.Set<PagePanel>()
                                      .FromSqlInterpolated($"EXEC GetAllPagePanels")
                                      .ToListAsync();

                return result ?? new List<PagePanel>();
            }
            catch (Exception ex)
            {
                return new List<PagePanel>();
            }
        }
    }

}
