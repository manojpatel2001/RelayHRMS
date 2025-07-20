using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
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

    public class ProjectDetailsRepository : IProjectDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public ProjectDetailsRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateProjectDetail(ProjectDetails model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageProjectDetails
                    @Action = {"CREATE"},
                    @ProjectName = {model.ProjectName},
                    @Description = {model.Description},
                    @CompanyId = {model.CompanyId},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateProjectDetail(ProjectDetails model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageProjectDetails
                    @Action = {"UPDATE"},
                    @ProjectDetailsId = {model.ProjectDetailsId},
                    @ProjectName = {model.ProjectName},
                    @Description = {model.Description},
                    @CompanyId = {model.CompanyId},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteProjectDetail(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageProjectDetails
                    @Action = {"DELETE"},
                    @ProjectDetailsId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<ProjectDetails?> GetProjectDetailById(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<ProjectDetails>().FromSqlInterpolated($@"
                EXEC GetProjectDetailById
                    @ProjectDetailsId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result?.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ProjectDetails>> GetAllProjectDetails(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<ProjectDetails>().FromSqlInterpolated($@"
                EXEC GetAllProjectDetails
                    @CompanyId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<ProjectDetails>();
            }
        }

    }
}
