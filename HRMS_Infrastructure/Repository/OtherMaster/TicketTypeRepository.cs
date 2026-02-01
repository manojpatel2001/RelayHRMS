using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.EntityFrameworkCore;


namespace HRMS_Infrastructure.Repository.OtherMaster
{
   
        public class TicketTypeRepository : ITicketTypeRepository
        {
            private readonly HRMSDbContext _db;

            public TicketTypeRepository(HRMSDbContext db)
            {
                _db = db;
            }

            public async Task<List<TicketType>> GetAllTicketTypes(int companyId)
            {
                try
                {
                    return await _db.Set<TicketType>()
                        .FromSqlInterpolated($"EXEC GetAllTicketTypes @CompanyId = {companyId}")
                        .ToListAsync();
                }
                catch
                {
                    return new List<TicketType>();
                }
            }

            public async Task<TicketType?> GetTicketTypeById(int ticketTypeId)
            {
                try
                {
                    var result = await _db.Set<TicketType>()
                        .FromSqlInterpolated($"EXEC GetTicketTypeById @TicketTypeId = {ticketTypeId}")
                        .ToListAsync();
                    return result.FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }

            public async Task<SP_Response> CreateTicketType(TicketType ticketType)
            {
                try
                {
                    var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketType
                        @Action = {"CREATE"},
                        @TicketTypeName = {ticketType.TicketTypeName},
                        @DepartmentId = {ticketType.DepartmentId},
                        @CompanyId = {ticketType.CompanyId},
                        @CreatedBy = {ticketType.CreatedBy}
                ").ToListAsync();
                    return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
                catch
                {
                    return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
                }
            }

            public async Task<SP_Response> UpdateTicketType(TicketType ticketType)
            {
                try
                {
                    var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketType
                        @Action = {"UPDATE"},
                        @TicketTypeId = {ticketType.TicketTypeId},
                        @TicketTypeName = {ticketType.TicketTypeName},
                        @DepartmentId = {ticketType.DepartmentId},
                        @CompanyId = {ticketType.CompanyId},
                        @UpdatedBy = {ticketType.UpdatedBy}
                ").ToListAsync();
                    return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
                catch
                {
                    return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
                }
            }

            public async Task<SP_Response> DeleteTicketType(DeleteRecordVM deleteRecord)
            {
                try
                {
                    var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketType
                        @Action = {"DELETE"},
                        @TicketTypeId = {deleteRecord.Id},
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
