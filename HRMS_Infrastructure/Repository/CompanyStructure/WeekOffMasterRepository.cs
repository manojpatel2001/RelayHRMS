﻿using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Infrastructure.Interface.CompanyStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.CompanyStructure
{
    public class WeekOffMasterRepository:Repository<WeekOffDetails>,IWeekOffMasterRepository
    {
        private HRMSDbContext _db;

        public WeekOffMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateWeekOffDetails(WeekOffDetails weekOffDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC CreateWeekOffDetails
                    @CompanyId = {weekOffDetails.CompanyId},
                    @BranchId = {weekOffDetails.BranchId},
                    @WeekOffDay = {weekOffDetails.WeekOffDay},                   
                    @WeekOffName = {weekOffDetails.WeekOffName},                   
                    @IsDeleted = {weekOffDetails.IsDeleted},
                    @IsEnabled = {weekOffDetails.IsEnabled},
                    @IsBlocked = {weekOffDetails.IsBlocked},
                    @CreatedDate = {weekOffDetails.CreatedDate},
                    @CreatedBy = {weekOffDetails.CreatedBy},
                    @UpdatedDate = {weekOffDetails.UpdatedDate},
                    @UpdatedBy = {weekOffDetails.UpdatedBy},
                    @DeletedDate = {weekOffDetails.DeletedDate},
                    @DeletedBy = {weekOffDetails.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = 0 };
            }

        }

        public async Task<VMCommonResult> DeleteWeekOffDetails(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC DeleteWeekOffDetails 
                        @WeekOffDetailsId = {deleteRecordVM.Id},
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

        public async Task<List<vmGetAllWeekOffDetails>> GetAllWeekOffDetails(vmCommonParameters vmCommonParameters)
        {
            try
            {
                var result = await _db.Set<vmGetAllWeekOffDetails>().FromSqlInterpolated($@"
            EXEC GetAllWeekOffDetails  
                    @CompanyId = {vmCommonParameters.CompanyId},
                    @BranchId = {vmCommonParameters.BranchId}").ToListAsync();

                return result ?? new List<vmGetAllWeekOffDetails>();
            }
            catch (Exception ex)
            {
                return new List<vmGetAllWeekOffDetails>();
            }
        }


        public async Task<VMCommonResult> UpdateWeekOffDetails(WeekOffDetails weekOffDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateWeekOffDetails 
                        @WeekOffDetailsId = {weekOffDetails.WeekOffDetailsId},
                        @CompanyId = {weekOffDetails.CompanyId},
                        @BranchId = {weekOffDetails.BranchId},
                        @WeekOffDay = {weekOffDetails.WeekOffDay},                                      
                        @WeekOffName = {weekOffDetails.WeekOffName},                                      
                        @UpdatedDate = {weekOffDetails.UpdatedDate},
                        @UpdatedBy = {weekOffDetails.UpdatedBy}
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
