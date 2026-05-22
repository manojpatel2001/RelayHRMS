using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class SkillMasterRepository : ISkillMasterRepository
    {
        private readonly HRMSDbContext _db;

        public SkillMasterRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<SP_Response> CreateSkillMaster(SkillMaster model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageSkillMaster
                        @Action = {"CREATE"},
                        @SkillName = {model.SkillName},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateSkillMaster(SkillMaster model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageSkillMaster
                        @Action = {"UPDATE"},
                        @SkillMasterId = {model.SkillMasterId},
                        @SkillName = {model.SkillName},
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteSkillMaster(DeleteRecordVM model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageSkillMaster
                        @Action = {"DELETE"},
                        @SkillMasterId = {model.Id},
                        @DeletedBy = {model.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<List<SkillMaster>> GetAllSkillMasters()
        {
            try
            {
                return await _db.Set<SkillMaster>()
                                .FromSqlInterpolated($"EXEC GetAllSkillMaster")
                                .ToListAsync();
            }
            catch
            {
                return new List<SkillMaster>();
            }
        }
    }
}
