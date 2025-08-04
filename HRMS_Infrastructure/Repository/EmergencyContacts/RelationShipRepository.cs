using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.EmegencyContacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmergencyContacts
{
    public class RelationShipRepository: IRelationShipRepository
    {
        private readonly HRMSDbContext _db;

        public RelationShipRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateRelationShip(RelationShip model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageRelationShip
                        @Action = {"CREATE"},
                        @RelationName = {model.RelationName},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateRelationShip(RelationShip model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageRelationShip
                        @Action = {"UPDATE"},
                        @RelationShipId = {model.RelationShipId},
                        @RelationName = {model.RelationName},
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteRelationShip(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageRelationShip
                        @Action = {"DELETE"},
                        @RelationShipId = {deleteRecord.Id},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }
        public async Task<List<RelationShip>> GetAllRelationShips(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<RelationShip>().FromSqlInterpolated($@"
                EXEC GetAllRelationShips
                    @RelationName= {vmCommonGetById.Title},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<RelationShip>();
            }
        }
        public async Task<EmergencyContact?> GetRelationShipById(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<EmergencyContact>().FromSqlInterpolated($@"
                EXEC GetRelationShipById
                    @RelationShipId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }


    }
}
