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
    public class TicketStatusRepository : ITicketStatusRepository
    {
        private readonly HRMSDbContext _db;

        public TicketStatusRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<TicketStatus>> GetAllTicketStatus()
        {
            try
            {
                return await _db.Set<TicketStatus>()
                    .FromSqlInterpolated($"EXEC GetAllTicketStatus")
                    .ToListAsync();
            }
            catch
            {
                return new List<TicketStatus>();
            }
        }

        public async Task<TicketStatus?> GetTicketStatusById(int ticketStatusId)
        {
            try
            {
                var result = await _db.Set<TicketStatus>()
                    .FromSqlInterpolated($"EXEC GetTicketStatusById @TicketStatusId = {ticketStatusId}")
                    .ToListAsync();
                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<SP_Response> CreateTicketStatus(TicketStatus ticketStatus)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketStatus
                        @Action = {"CREATE"},
                        @TicketStatusName = {ticketStatus.TicketStatusName},
                        @IsEnabled = {ticketStatus.IsEnabled},
                        @IsDeleted = {ticketStatus.IsDeleted}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateTicketStatus(TicketStatus ticketStatus)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketStatus
                        @Action = {"UPDATE"},
                        @TicketStatusId = {ticketStatus.TicketStatusId},
                        @TicketStatusName = {ticketStatus.TicketStatusName},
                        @IsEnabled = {ticketStatus.IsEnabled}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteTicketStatus(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketStatus
                        @Action = {"DELETE"},
                        @TicketStatusId = {deleteRecord.Id}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

       
    }
}
