﻿using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.VM;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class CompOffDetailsRepository:Repository<Comp_Off_Details>,ICompOffDetailsRepository
    {

        private HRMSDbContext _db;

        public CompOffDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<VMCompOffDetails>> GetCompOffApplicationsAsync(SearchVmCompOff filter)
        {
            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@SearchType", string.IsNullOrEmpty(filter.SearchType) ? DBNull.Value : filter.SearchType),
                    new SqlParameter("@SearchFor", string.IsNullOrEmpty(filter.SearchFor) ? DBNull.Value : filter.SearchFor),
                    new SqlParameter("@Status", string.IsNullOrEmpty(filter.Status) ? DBNull.Value : filter.Status),
                    new SqlParameter("@EmpId", (object?)filter.Emplooyeid ?? DBNull.Value),
                    new SqlParameter("@CompId", (object?)filter.CompId ?? DBNull.Value),
                };

                return await _db.Set<VMCompOffDetails>()
                    .FromSqlRaw("EXEC GetCompOffApplications @SearchType, @SearchFor, @Status, @EmpId,@CompId", parameters)
                .ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine("GetCompOffApplicationsAsync Error: " + ex.Message);
                return new List<VMCompOffDetails>();
            }
        }

        public async Task<SP_Response> InsertCompOffAsync(Comp_Off_Details model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                EXEC usp_InsertCompOffDetails
                    @Cmp_Id = {model.Cmp_Id},
                    @Emp_Id = {model.Emp_Id},
                    @Emp_Code = {model.Emp_Code},
                    @Rep_Person_Id = {model.Rep_Person_Id},
                    @ApplicationDate = {model.ApplicationDate},
                    @Extra_Work_Day = {model.Extra_Work_Day},
                    @Extra_Work_Hours = {model.Extra_Work_Hours},
                    @Application_Status = {model.Application_Status},
                    @Comp_Off_Type = {model.Comp_Off_Type},
                    @CreatedBy = {model.CreatedBy},
                    @ComoffReason = {model.ComoffReason}
            ").ToListAsync();

                return result.FirstOrDefault()
                       ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateCompOffApproval(ApproveandrejectVM compOffVM)
        {
            try
            {
                string idsString = string.Join(",", compOffVM.CompoffIds);

                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                EXEC UpdateCompOffApproval
                @compOffDetailsIds = {idsString},
                @status = {compOffVM.Status},
                @ApprovedBy = {compOffVM.EmployeeId.ToString()}
            ")
                    .ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong" };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }


        //public Task<bool> UpdateLeaveMange(List<int> ids, string status)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> UpdateLeaveManger(List<int> comoffid, string status)
        {

            try
            {
                if(status== "Approved")
                {
                    foreach (var id in comoffid)
                    {


                        var parameters = new[]
                        {
                        new SqlParameter("@Comp_Off_Id", id),

                    };

                        await _db.Database.ExecuteSqlRawAsync(
                            "EXEC sp_UpdateCompOffLeaveFromDetails @Comp_Off_Id",
                            parameters
                        );
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during SP call: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateLeavedetails(List<int> ids, string status)
        {

            try
            {
                if (status == "Approved")
                {
                    foreach (var id in ids)
                    {


                        var parameters = new[]
                        {
                        new SqlParameter("@Id", id),

                    };

                        await _db.Database.ExecuteSqlRawAsync(
                            "EXEC sp_DeductLeaveBalance @Id",
                            parameters
                        );
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during SP call: " + ex.Message);
                return false;
            }
        }



        public async Task<List<VMCompOffDetails>> GetCompOffApplicationsAdmin(SearchVmCompOff filter)
        {
            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@SearchType", string.IsNullOrEmpty(filter.SearchType) ? DBNull.Value : filter.SearchType),
                    new SqlParameter("@SearchFor", string.IsNullOrEmpty(filter.SearchFor) ? DBNull.Value : filter.SearchFor),
                    new SqlParameter("@Status", string.IsNullOrEmpty(filter.Status) ? DBNull.Value : filter.Status),
                    new SqlParameter("@EmpId", (object?)filter.Emplooyeid ?? DBNull.Value),
                    new SqlParameter("@CompId", (object?)filter.CompId ?? DBNull.Value),
                };

                return await _db.Set<VMCompOffDetails>()
                    .FromSqlRaw("EXEC GetCompOffApplicationsAdmin @SearchType, @SearchFor, @Status, @EmpId,@CompId", parameters)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCompOffApplicationsAsync Error: " + ex.Message);
                return new List<VMCompOffDetails>();
            }
        }

        public async Task<Comp_Off_Details?> GetCompOffApplicationById(int Comp_Off_DetailsId)
        {
            try
            {
                var result = await _db.Set<Comp_Off_Details>()
                    .FromSqlInterpolated($"EXEC GetCompOffApplicationById @Comp_Off_DetailsId = {Comp_Off_DetailsId}")
                    .ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

    }
}
