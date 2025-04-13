using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
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
    public class TicketPriorityRepository:Repository<TicketPriority>,ITicketPriorityRepository
    {
        private HRMSDbContext _db;

        public TicketPriorityRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateTicketPriority(TicketPriority ticketPriority)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC CreateTicketPriority 
                    @TicketPriorityName = {ticketPriority.TicketPriorityName},
                    @EscalationHours = {ticketPriority.EscalationHours},
                    @IsDeleted = {ticketPriority.IsDeleted},
                    @IsEnabled = {ticketPriority.IsEnabled},
                    @IsBlocked = {ticketPriority.IsBlocked},
                    @CreatedDate = {ticketPriority.CreatedDate},
                    @CreatedBy = {ticketPriority.CreatedBy},
                    @DeletedDate = {ticketPriority.DeletedDate},
                    @DeletedBy = {ticketPriority.DeletedBy},
                    @UpdatedDate = {ticketPriority.UpdatedDate},
                    @UpdatedBy = {ticketPriority.UpdatedBy}
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

        public async Task<VMCommonResult> DeleteTicketPriority(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC DeleteTicketPriority 
                    @TicketPriorityId = {deleteRecordVM.Id},
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

        public async Task<VMCommonResult> UpdateTicketPriority(TicketPriority ticketPriority)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                 EXEC UpdateTicketPriority 
                     @TicketPriorityId= {ticketPriority.TicketPriorityId},
                     @TicketPriorityName = {ticketPriority.TicketPriorityName},
                     @EscalationHours = {ticketPriority.EscalationHours},
                     @UpdatedDate = {ticketPriority.UpdatedDate},
                     @UpdatedBy = {ticketPriority.UpdatedBy}
                    
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
