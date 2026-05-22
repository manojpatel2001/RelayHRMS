using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Infrastructure.Repository.TicketManagement
{
    public class TicketPriorityRepository : ITicketPriorityRepository
    {
        private readonly HRMSDbContext _db;

        public TicketPriorityRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<TicketPriority>> GetAllTicketPriority(int companyId)
        {
            try
            {
                return await _db.Set<TicketPriority>()
                    .FromSqlInterpolated($"EXEC GetAllTicketPriority @CompanyId = {companyId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<TicketPriority>();
            }
        }

        public async Task<TicketPriority?> GetTicketPriorityById(int ticketPriorityId)
        {
            try
            {
                var result = await _db.Set<TicketPriority>()
                    .FromSqlInterpolated($"EXEC GetTicketPriorityById @TicketPriorityId = {ticketPriorityId}")
                    .ToListAsync();
                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<SP_Response> CreateTicketPriority(TicketPriority ticketPriority)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketPriority
                        @Action = {"CREATE"},
                        @TicketPriorityName = {ticketPriority.TicketPriorityName},
                        @EscalationHours = {ticketPriority.EscalationHours},
                        @CompanyId = {ticketPriority.CompanyId},
                        @CreatedBy = {ticketPriority.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateTicketPriority(TicketPriority ticketPriority)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketPriority
                        @Action = {"UPDATE"},
                        @TicketPriorityId = {ticketPriority.TicketPriorityId},
                        @TicketPriorityName = {ticketPriority.TicketPriorityName},
                        @EscalationHours = {ticketPriority.EscalationHours},
                        @CompanyId = {ticketPriority.CompanyId},
                        @UpdatedBy = {ticketPriority.UpdatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteTicketPriority(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketPriority
                        @Action = {"DELETE"},
                        @TicketPriorityId = {deleteRecord.Id},
                        @UpdatedBy = {deleteRecord.DeletedBy}
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
