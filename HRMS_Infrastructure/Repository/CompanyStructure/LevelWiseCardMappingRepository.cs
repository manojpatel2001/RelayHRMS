using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.CompanyStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.CompanyStructure
{
    public class LevelWiseCardMappingRepository : Repository<LevelWiseCardMapping>,ILevelWiseCardMappingRepository
    {
        private HRMSDbContext _db;

        public LevelWiseCardMappingRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<LevelWiseCardMapping>> GetAllLevelWiseCardMapping()
        {
            try
            {
                var result = await _db.Set<LevelWiseCardMapping>()
                                      .FromSqlInterpolated($"EXEC GetAllLevelWiseCardMapping")
                                      .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<LevelWiseCardMapping>();
            }
        }

        public async Task<LevelWiseCardMapping?> GetByLevelWiseCardMappingId(int levelWiseCardMappingId)
        {
            try
            {
                var result = await _db.Set<LevelWiseCardMapping>()
                                      .FromSqlInterpolated($"EXEC GetLevelWiseCardMappingById @LevelWiseCardMappingId = {levelWiseCardMappingId}")
                                      .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateLevelWiseCardMapping(LevelWiseCardMapping levelWiseCardMapping)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageLevelWiseCardMapping
                        @Action = {"CREATE"},
                        @Level = {levelWiseCardMapping.Level},
                        @CardType = {levelWiseCardMapping.CardType},
                        @CardNo = {levelWiseCardMapping.CardNo},
                        @IsDeleted = {levelWiseCardMapping.IsDeleted},
                        @IsEnabled = {levelWiseCardMapping.IsEnabled},
                        @IsBlocked = {levelWiseCardMapping.IsBlocked},
                        @CreatedDate = {levelWiseCardMapping.CreatedDate},
                        @CreatedBy = {levelWiseCardMapping.CreatedBy},
                        @UpdatedDate = {levelWiseCardMapping.UpdatedDate},
                        @UpdatedBy = {levelWiseCardMapping.UpdatedBy},
                        @DeletedDate = {levelWiseCardMapping.DeletedDate},
                        @DeletedBy = {levelWiseCardMapping.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateLevelWiseCardMapping(LevelWiseCardMapping levelWiseCardMapping)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageLevelWiseCardMapping
                        @Action = {"UPDATE"},
                        @LevelWiseCardMappingId = {levelWiseCardMapping.LevelWiseCardMappingId},
                        @Level = {levelWiseCardMapping.Level},
                        @CardType = {levelWiseCardMapping.CardType},
                        @CardNo = {levelWiseCardMapping.CardNo},
                        @UpdatedDate = {levelWiseCardMapping.UpdatedDate},
                        @UpdatedBy = {levelWiseCardMapping.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteLevelWiseCardMapping(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageLevelWiseCardMapping
                        @Action = {"DELETE"},
                        @LevelWiseCardMappingId = {deleteRecordVM.Id},
                        @DeletedDate = {deleteRecordVM.DeletedDate},
                        @DeletedBy = {deleteRecordVM.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

       
    }
}
