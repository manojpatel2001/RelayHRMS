using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class TicketFollowUpRepository : ITicketFollowUpRepository
    {
        private readonly HRMSDbContext _db;

        public TicketFollowUpRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<TicketFollowUp>> GetAllTicketFollowUpByApplicationId(int ticketApplicationId)
        {
            try
            {
                return await _db.Set<TicketFollowUp>()
                    .FromSqlInterpolated($"EXEC GetAllTicketFollowUpByApplicationId @TicketApplicationId = {ticketApplicationId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<TicketFollowUp>();
            }
        }

        public async Task<SP_Response> CreateTicketFollowUp(vmTicketFollowUp ticketFollowUp)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketFollowUp
                        @Action = {"CREATE"},
                        @TicketFollowRemark = {ticketFollowUp.TicketFollowRemark},
                        @TicketFollowDate = {ticketFollowUp.TicketFollowDate},
                        @NextTicketFollowDate = {ticketFollowUp.NextTicketFollowDate},
                        @FollowUpDocumentUrl = {ticketFollowUp.FollowUpDocumentUrl},
                        @TaggedUsers = {ticketFollowUp.TaggedUsers},
                        @EmployeeId = {ticketFollowUp.EmployeeId},
                        @TicketApplicationId = {ticketFollowUp.TicketApplicationId},
                        @TicketStatusId = {ticketFollowUp.TicketStatusId},
                        @CreatedBy = {ticketFollowUp.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateTicketFollowUp(vmTicketFollowUp ticketFollowUp)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketFollowUp
                        @Action = {"UPDATE"},
                        @TicketFollowUpId = {ticketFollowUp.TicketFollowUpId},
                        @TicketFollowRemark = {ticketFollowUp.TicketFollowRemark},
                        @TicketFollowDate = {ticketFollowUp.TicketFollowDate},
                        @NextTicketFollowDate = {ticketFollowUp.NextTicketFollowDate},
                        @FollowUpDocumentUrl = {ticketFollowUp.FollowUpDocumentUrl},
                        @TaggedUsers = {ticketFollowUp.TaggedUsers},
                        @TicketApplicationId = {ticketFollowUp.TicketApplicationId},
                        @TicketStatusId = {ticketFollowUp.TicketStatusId},
                        @UpdatedBy = {ticketFollowUp.UpdatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteTicketFollowUp(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketFollowUp
                        @Action = {"DELETE"},
                        @TicketFollowUpId = {deleteRecord.Id},
                        @UpdatedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<TicketFollowUp?> GetTicketFollowUpById(int ticketFollowUpId)
        {

            try
            {
                var result= await _db.Set<TicketFollowUp>().FromSqlInterpolated($"EXEC GetTicketFollowUpById @TicketFollowUpId = {ticketFollowUpId}").ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }
    }
}
