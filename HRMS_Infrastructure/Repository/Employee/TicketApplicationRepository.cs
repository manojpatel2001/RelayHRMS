using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class TicketApplicationRepository : ITicketApplicationRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public TicketApplicationRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<TicketApplication>> GetAllTicketApplications(CommonParameter common)
        {
            try
            {
                return await _db.Set<TicketApplication>()
                    .FromSqlInterpolated($"EXEC GetAllTicketApplications @CompanyId = {common.CompanyId},@EmployeeId={common.EmployeeId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<TicketApplication>();
            }
        }

        public async Task<TicketApplication?> GetTicketApplicationById(int ticketApplicationId)
        {
            try
            {
                var result = await _db.Set<TicketApplication>()
                    .FromSqlInterpolated($"EXEC GetTicketApplicationById @TicketApplicationId = {ticketApplicationId}")
                    .ToListAsync();
                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<SP_Response> CreateTicketApplication(vmTicketApplication ticketApplication)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketApplication
                        @Action = {"CREATE"},
                        @TicketDate = {ticketApplication.TicketDate},
                        @DepartmentId = {ticketApplication.DepartmentId},
                        @TicketTypeId = {ticketApplication.TicketTypeId},
                        @TicketAssignId = {ticketApplication.TicketAssignId},
                        @EmployeeId = {ticketApplication.EmployeeId},
                        @TicketDescription = {ticketApplication.TicketDescription},
                        @TicketPriorityId = {ticketApplication.TicketPriorityId},
                        @AttachDocumentUrl = {ticketApplication.AttachDocumentUrl},
                        @TicketStatusId = {ticketApplication.TicketStatusId},
                        @CompanyId = {ticketApplication.CompanyId},
                        @CreatedBy = {ticketApplication.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateTicketApplication(vmTicketApplication ticketApplication)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketApplication
                        @Action = {"UPDATE"},
                        @TicketApplicationId = {ticketApplication.TicketApplicationId},
                        @TicketDate = {ticketApplication.TicketDate},
                        @DepartmentId = {ticketApplication.DepartmentId},
                        @TicketTypeId = {ticketApplication.TicketTypeId},
                        @TicketAssignId = {ticketApplication.TicketAssignId},
                        @EmployeeId = {ticketApplication.EmployeeId},
                        @TicketDescription = {ticketApplication.TicketDescription},
                        @TicketPriorityId = {ticketApplication.TicketPriorityId},
                        @AttachDocumentUrl = {ticketApplication.AttachDocumentUrl},
                        @TicketStatusId = {ticketApplication.TicketStatusId},
                        @CompanyId = {ticketApplication.CompanyId},
                        @UpdatedBy = {ticketApplication.UpdatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteTicketApplication(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageTicketApplication
                        @Action = {"DELETE"},
                        @TicketApplicationId = {deleteRecord.Id},
                        @UpdatedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<(List<vmEmployeeListDto> Employees, List<vmTicketTypeDto> TicketTypes)> GetEmployeeAndTicketTypeByDepartmentId(CommonParameter commonParameter)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@DepartmentId", commonParameter.DepartmentId);
                    queryParameters.Add("@CompanyId", commonParameter.CompanyId);

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetEmployeeAndTicketTypeByDepartmentId",
                        queryParameters,
                        commandType: CommandType.StoredProcedure))
                    {
                        try
                        {
                            var employees = (await multi.ReadAsync<vmEmployeeListDto>()).AsList();
                            var ticketTypes = (await multi.ReadAsync<vmTicketTypeDto>()).AsList();

                            return (employees, ticketTypes);
                        }
                        catch (Exception ex)
                        {
                            return (new List<vmEmployeeListDto>(), new List<vmTicketTypeDto>());
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                return (new List<vmEmployeeListDto>(), new List<vmTicketTypeDto>());
            }
        }

        public async Task<List<TicketApplication>> GetAllAssignTicketApplications(CommonParameter common)
        {
            try
            {
                return await _db.Set<TicketApplication>()
                    .FromSqlInterpolated($"EXEC GetAllAssignTicketApplications @CompanyId = {common.CompanyId},@EmployeeId={common.EmployeeId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<TicketApplication>();
            }
        }

    }
}
