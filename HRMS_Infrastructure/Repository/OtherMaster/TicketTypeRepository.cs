using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class TicketTypeRepository:Repository<TicketType>,ITicketTypeRepository
    {
        private HRMSDbContext _db;

        public TicketTypeRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateTicketType(TicketType ticketType)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC CreateTicketType 
                    @TicketTypeName = {ticketType.TicketTypeName},
                    @DepartmentId = {ticketType.DepartmentId},
                    @IsDeleted = {ticketType.IsDeleted},
                    @IsEnabled = {ticketType.IsEnabled},
                    @IsBlocked = {ticketType.IsBlocked},
                    @CreatedDate = {ticketType.CreatedDate},
                    @CreatedBy = {ticketType.CreatedBy},
                    @DeletedDate = {ticketType.DeletedDate},
                    @DeletedBy = {ticketType.DeletedBy},
                    @UpdatedDate = {ticketType.UpdatedDate},
                    @UpdatedBy = {ticketType.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0,
                };
            }
            catch (Exception ex)
            {

                return new VMCommonResult
                {
                    Id = 0,
                };
            }
        }

        public async  Task<VMCommonResult> DeleteTicketType(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC DeleteTicketType 
                    @TicketTypeId = {deleteRecordVM.Id},
                    @DeletedDate = {deleteRecordVM.DeletedDate},
                    @DeletedBy = {deleteRecordVM.DeletedBy}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0,
                };
            }
            catch (Exception ex)
            {

                return new VMCommonResult
                {
                    Id = 0,
                };
            }
        }

        public async Task<List<vmGetAllTicketTypes>> GetAllTicketTypes()
        {
            try
            {
                var result = await _db.Set<vmGetAllTicketTypes>()
                                      .FromSqlInterpolated($@"EXEC GetAllTicketTypes")
                                      .ToListAsync();

                return result ?? new List<vmGetAllTicketTypes>();
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return new List<vmGetAllTicketTypes>(); // return empty list on error
            }
        }


        public async Task<VMCommonResult> UpdateTicketType(TicketType ticketType)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC UpdateTicketType 
                    @TicketTypeId = {ticketType.TicketTypeId},
                    @TicketTypeName = {ticketType.TicketTypeName},
                    @DepartmentId = {ticketType.DepartmentId},
                    @UpdatedDate = {ticketType.UpdatedDate},
                    @UpdatedBy = {ticketType.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0,
                };
            }
            catch (Exception ex)
            {

                return new VMCommonResult
                {
                    Id = 0,
                };
            }
        }
    }
}
