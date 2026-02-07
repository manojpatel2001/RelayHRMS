using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class NewsAnnouncementRepository:Repository<NewsAnnouncement>,INewsAnnouncementRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public NewsAnnouncementRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<SP_Response> CreateNewsAnnouncement(NewsAnnouncement model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@CmpID", model.CmpID);
                    parameters.Add("@NewsTitle", model.NewsTitle);
                    parameters.Add("@NewsDescription", model.NewsDescription);
                    parameters.Add("@StartDate", model.StartDate);
                    parameters.Add("@EndDate", model.EndDate);
                    parameters.Add("@IsVisible", model.IsVisible);
                    parameters.Add("@IsThought", model.IsThought);
                    parameters.Add("@IsPop", model.IsPop);
                    parameters.Add("@IsLoginNotification", model.IsLoginNotification);
                    parameters.Add("@BranchWiseNewsAnnoun", model.BranchWiseNewsAnnoun);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@IsDeleted", model.IsDeleted);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_NewsAnnouncement_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response
                    {
                        Success = 0,
                        ResponseMessage = "No response from database"
                    };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<List<NewsAnnouncement>> GetNewsAnnouncement(int? CompanyId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompId", CompanyId),

                };

                var result = await _db.Set<NewsAnnouncement>()
                    .FromSqlRaw("EXEC GetNewsAnnouncements @CompId ", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<NewsAnnouncement>();
            }
        }

        public async Task<SP_Response> UpdateNewsAnnouncement(NewsAnnouncement model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@NewsID", model.NewsID);
                    parameters.Add("@CmpID", model.CmpID);
                    parameters.Add("@NewsTitle", model.NewsTitle);
                    parameters.Add("@NewsDescription", model.NewsDescription);
                    parameters.Add("@StartDate", model.StartDate);
                    parameters.Add("@EndDate", model.EndDate);
                    parameters.Add("@IsVisible", model.IsVisible);
                    parameters.Add("@IsThought", model.IsThought);
                    parameters.Add("@IsPop", model.IsPop);
                    parameters.Add("@IsLoginNotification", model.IsLoginNotification);
                    parameters.Add("@BranchWiseNewsAnnoun", model.BranchWiseNewsAnnoun);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_NewsAnnouncement_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response
                    {
                        Success = 0,
                        ResponseMessage = "No response from database"
                    };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SP_Response> DeleteNewsAnnouncement(DeleteRecordVM model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "DELETE");
                    parameters.Add("@NewsID", model.Id);
                    parameters.Add("@DeletedBy", model.DeletedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_NewsAnnouncement_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response
                    {
                        Success = 0,
                        ResponseMessage = "No response from database"
                    };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }
    }
}
