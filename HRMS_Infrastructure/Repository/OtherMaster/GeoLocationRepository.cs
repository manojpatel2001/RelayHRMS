using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class GeoLocationRepository : IGeoLocationRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public GeoLocationRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }


        public async Task<List<GeoLocation>> GetAllGeoLocations(int companyId)
        {
            try
            {
                return await _db.Set<GeoLocation>()
                    .FromSqlInterpolated($"EXEC GetAllGeolocations @CompanyId = {companyId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<GeoLocation>();
            }
        }

       

        public async Task<SP_Response> CreateGeoLocation(GeoLocation geoLocation)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageGeoLocation
                        @Action = {"CREATE"},
                        @BranchId = {geoLocation.BranchId},
                        @Latitude = {geoLocation.Latitude},
                        @Longitude = {geoLocation.Longitude},
                        @Meter = {geoLocation.Meter},
                        @CompanyId = {geoLocation.CompanyId},
                        @CreatedBy = {geoLocation.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateGeoLocation(GeoLocation geoLocation)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageGeoLocation
                        @Action = {"UPDATE"},
                        @GeoLocationId = {geoLocation.GeoLocationId},
                        @BranchId = {geoLocation.BranchId},
                        @Latitude = {geoLocation.Latitude},
                        @Longitude = {geoLocation.Longitude},
                        @Meter = {geoLocation.Meter},
                        @CompanyId = {geoLocation.CompanyId},
                        @UpdatedBy = {geoLocation.UpdatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteGeoLocation(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageGeoLocation
                        @Action = {"DELETE"},
                        @GeoLocationId = {deleteRecord.Id},
                        @UpdatedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> CreateAssignGeoLocation(AssignGeoLocation geoLocation)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageAssignGeoLocation
                        @Action = {"CREATE"},
                        @GeoLocationIds = {geoLocation.GeoLocationIds},
                        @EmployeeIds = {geoLocation.EmployeeIds},
                      
                        @CreatedBy = {geoLocation.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }
        public async Task<SP_Response> DeleteAssignGeoLocation(AssignGeoLocation deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageAssignGeoLocation
                        @Action = {"DELETE"},
                        @EmployeeIds = {deleteRecord.EmployeeIds},
                        @CreatedBy = {deleteRecord.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<List<GetAllAssignGeoLocation>> GetAssignGeoLocationsWithLocation(int companyId)
        {
            try
            {
                return await _db.Set<GetAllAssignGeoLocation>()
                    .FromSqlInterpolated($"EXEC GetAssignGeoLocationsWithLocation @CompanyId = {companyId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<GetAllAssignGeoLocation>();
            }
        }

        public async Task<(List<vmEmployeeListDto> Employees, List<GeoLocation> Locations)> GetAllEmployeeAndLocation(CommonParameter commonParameter)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CompanyId", commonParameter.CompanyId);

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetAllEmployeeAndLocation",
                        queryParameters,
                        commandType: CommandType.StoredProcedure))
                    {
                        try
                        {
                            var employees = (await multi.ReadAsync<vmEmployeeListDto>()).AsList();
                            var locations = (await multi.ReadAsync<GeoLocation>()).AsList();

                            return (employees, locations);
                        }
                        catch (Exception ex)
                        {
                            return (new List<vmEmployeeListDto>(), new List<GeoLocation>());
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                return (new List<vmEmployeeListDto>(), new List<GeoLocation>());
            }
        }

    }
}
