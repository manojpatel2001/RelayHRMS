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
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "CREATE");
                    parameters.Add("@BranchId", geoLocation.BranchId);
                    parameters.Add("@Latitude", geoLocation.Latitude, DbType.Decimal);
                    parameters.Add("@Longitude", geoLocation.Longitude, DbType.Decimal);
                    parameters.Add("@Meter", geoLocation.Meter);
                    parameters.Add("@CompanyId", geoLocation.CompanyId);
                    parameters.Add("@CreatedBy", geoLocation.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "ManageGeoLocation",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
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
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "UPDATE");
                    parameters.Add("@GeoLocationId", geoLocation.GeoLocationId);
                    parameters.Add("@BranchId", geoLocation.BranchId);
                    parameters.Add("@Latitude", geoLocation.Latitude, DbType.Decimal);
                    parameters.Add("@Longitude", geoLocation.Longitude, DbType.Decimal);
                    parameters.Add("@Meter", geoLocation.Meter);
                    parameters.Add("@CompanyId", geoLocation.CompanyId);
                    parameters.Add("@UpdatedBy", geoLocation.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "ManageGeoLocation",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
                }
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
        public async Task<SP_Response> UpdateAssignGeoLocation(AssignGeoLocation geoLocation)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageAssignGeoLocation
                        @Action = {"UPDATE"},
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

                            // "Location Assign to Employee" should offer every company employee
                            // who has Mobile Access on (Geo Fencing itself is not a prerequisite —
                            // assigning a zone is how Geo Fencing gets enabled for them). The
                            // GetAllEmployeeAndLocation stored procedure already filters on
                            // IsMobileAccess too; this is a redundant-but-harmless second check.
                            var eligibleEmployeeIds = await _db.HRMSUserIdentities
                                .Where(u => u.IsMobileAccess == true)
                                .Select(u => u.Id)
                                .ToListAsync();

                            employees = employees
                                .Where(e => e.Id.HasValue && eligibleEmployeeIds.Contains(e.Id.Value))
                                .ToList();

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
