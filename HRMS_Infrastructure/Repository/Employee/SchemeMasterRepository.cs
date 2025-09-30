using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.Migrations;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class SchemeMasterRepository : Repository<SchemeMasterRepository>, ISchemeMasterRepository
    {
        private readonly HRMSDbContext _db;

        public SchemeMasterRepository(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }

        public Task AddAsync(SchemeMasterViewModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<SP_Response> AddSchemeMaster(SchemeMasterViewModel model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC sp_SchemeMaster_CRUD
                @Operation = {"INSERT"},
                @SchemeName = {model.SchemeName},
                @Type = {model.Type},
                @IsDefaultScheme = {model.IsDefaultScheme},
                @IsDeleted = {model.IsDeleted},
                @IsEnabled = {model.IsEnabled},
                @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response
                {
                    Success = 0,
                    ResponseMessage = "Something went wrong!"
                };
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SP_Response> DeleteSchemeMaster(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC sp_SchemeMaster_CRUD
                    @Operation = {"DELETE"},
                    @SchemeID = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public Task<IEnumerable<SchemeMasterViewModel>> GetAllAsync(Expression<Func<SchemeMasterViewModel, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SchemeMasterViewModel>> GetAllSchemeMaster(string SearchFor, string SearchBy)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@SearchFor", SearchFor ?? (object)DBNull.Value),
            new SqlParameter("@SearchBy", SearchBy ?? (object)DBNull.Value)
        };

                var result = await _db.Database
                    .SqlQueryRaw<SchemeMasterViewModel>($@"
                EXEC sp_GetAllSchemeMasters @SearchFor, @SearchBy",
                        parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
               
                return new List<SchemeMasterViewModel>();
            }
        }


        public Task<SchemeMasterViewModel> GetAsync(Expression<Func<SchemeMasterViewModel, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public async Task<SchemeMasterViewModel?> GetBySchemeId(int Id)
        {
            try
            {
                var result = await _db.Set<SchemeMasterViewModel>().FromSqlInterpolated($@"
                 EXEC sp_SchemeMaster_CRUD
                    @Operation = {"GET"},
                    @SchemeID = {Id}
                   
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public Task RemoveAsync(SchemeMasterViewModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<SP_Response> UpdateSchemeMaster(SchemeMasterViewModel model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC sp_SchemeMaster_CRUD
                @Operation = {"UPDATE"},
                @SchemeID = {model.SchemeID},
                @SchemeName = {model.SchemeName},
                @Type = {model.Type},
                @IsDefaultScheme = {model.IsDefaultScheme},
                @IsDeleted = {model.IsDeleted},
                @IsEnabled = {model.IsEnabled},
                @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response
                {
                    Success = 0,
                    ResponseMessage = "Something went wrong!"
                };
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }
    }
}
