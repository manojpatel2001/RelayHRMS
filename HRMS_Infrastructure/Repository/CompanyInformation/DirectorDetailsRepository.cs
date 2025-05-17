using HRMS_Core.DbContext;
using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.CompanyInformation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS_Core.VM.CompanyInformation;

namespace HRMS_Infrastructure.Repository.CompanyInformation
{
    public class DirectorDetailsRepository : Repository<DirectorDetails>, IDirectorDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public DirectorDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<vmGetAllDirectorDetails>> GetAllDirectorDetails()
        {
            try
            {
                return await _db.Set<vmGetAllDirectorDetails>()
                                .FromSqlInterpolated($"EXEC GetAllDirectorDetails")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllDirectorDetails>();
            }
        }

        public async Task<vmGetAllDirectorDetails?> GetByDirectorDetailsId(int directorDetailsId)
        {
            try
            {
                var result = await _db.Set<vmGetAllDirectorDetails>()
                                      .FromSqlInterpolated($"EXEC GetByDirectorDetailsId @DirectorDetailsId = {directorDetailsId}")
                                      .ToListAsync();

                return result.FirstOrDefault()??null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<vmGetAllDirectorDetails>> GetDirectorDetailsByCompanyId(int CompanyId)
        {
            try
            {
                var result = await _db.Set<vmGetAllDirectorDetails>()
                                      .FromSqlInterpolated($"EXEC GetDirectorDetailsByCompanyId @CompanyId = {CompanyId}")
                                      .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<vmGetAllDirectorDetails>();
            }
        }

        public async Task<VMCommonResult> CreateDirectorDetails(DirectorDetails directorDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageDirectorDetails
                        @Action = {"CREATE"},
                        @DirectorName = {directorDetails.DirectorName},
                        @DirectorAddress = {directorDetails.DirectorAddress},
                        @DirectorDOB = {directorDetails.DirectorDOB},
                        @DirectorBranch = {directorDetails.DirectorBranch},
                        @DirectorDesignation = {directorDetails.DirectorDesignation},
                        @CompanyId = {directorDetails.CompanyId},
                        @IsDeleted = {directorDetails.IsDeleted},
                        @IsEnabled = {directorDetails.IsEnabled},
                        @CreatedDate = {directorDetails.CreatedDate},
                        @CreatedBy = {directorDetails.CreatedBy},
                        @DeletedBy = {directorDetails.IsEnabled},
                        @DeletedDate = {directorDetails.DeletedDate},
                        @IsBlocked = {directorDetails.IsBlocked}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateDirectorDetails(DirectorDetails directorDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageDirectorDetails
                        @Action = {"UPDATE"},
                        @DirectorDetailsId = {directorDetails.DirectorDetailsId},
                        @DirectorName = {directorDetails.DirectorName},
                        @DirectorAddress = {directorDetails.DirectorAddress},
                        @DirectorDOB = {directorDetails.DirectorDOB},
                        @DirectorBranch = {directorDetails.DirectorBranch},
                        @DirectorDesignation = {directorDetails.DirectorDesignation},
                        @CompanyId = {directorDetails.CompanyId},
                        @UpdatedDate = {directorDetails.UpdatedDate},
                        @UpdatedBy = {directorDetails.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteDirectorDetails(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageDirectorDetails
                        @Action = {"DELETE"},
                        @DirectorDetailsId = {deleteRecordVM.Id},
                        @DeletedDate = {deleteRecordVM.DeletedDate},
                        @DeletedBy = {deleteRecordVM.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }
    }
}
