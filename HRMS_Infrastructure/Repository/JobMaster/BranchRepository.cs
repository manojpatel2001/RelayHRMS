using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private HRMSDbContext _db;

        public BranchRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateBranch(Branch model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageBranch
                    @Action = {"CREATE"},
                    @BranchName = {model.BranchName},
                    @BranchCode = {model.BranchCode},
                    @Address = {model.Address},
                    @CityId = {model.CityId},
                    @CountryName = {model.CountryName},
                    @StateId = {model.StateId},
                    @GSTIN_No = {model.GSTIN_No},
                    @IsActive = {model.IsActive},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateBranch(Branch model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageBranch
                    @Action = {"UPDATE"},
                    @BranchId = {model.BranchId},
                    @BranchName = {model.BranchName},
                    @BranchCode = {model.BranchCode},
                    @Address = {model.Address},
                    @CityId = {model.CityId},
                    @CountryName = {model.CountryName},
                    @StateId = {model.StateId},
                    @GSTIN_No = {model.GSTIN_No},
                    @IsActive = {model.IsActive},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteBranch(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageBranch
                    @Action = {"DELETE"},
                    @BranchId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<vmGetAllBranches>> GetAllBranches(vmCommonGetById filters)
        {
            try
            {
                var result = await _db.Set<vmGetAllBranches>().FromSqlInterpolated($@"
                EXEC GetAllBranches
                    @IsDeleted = {filters.IsDeleted},
                    @IsEnabled = {filters.IsEnabled},
                    @BranchName = {filters.Title}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<vmGetAllBranches>();
            }
        }

        public async Task<Branch?> GetBranchById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<Branch>().FromSqlInterpolated($@"
                EXEC GetBranchById
                    @BranchId = {filter.Id},
                    @IsDeleted = {filter.IsDeleted},
                    @IsEnabled = {filter.IsEnabled}
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<vmGetAllCityByStateId>> GetAllCityByStateId(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<vmGetAllCityByStateId>().FromSqlInterpolated($@"
                EXEC GetAllCityByStateId
                    @StateId = {filter.Id}
                    
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<vmGetAllCityByStateId>();
            }
        }

        public async Task<List<BranchUserStatsModel>> GetBranchWiseEmpCount( int CompanyId)
        {
            try
            {
                return await _db.Set<BranchUserStatsModel>().FromSqlInterpolated($"EXEC GetBranchUserStats @CompanyId={CompanyId}").ToListAsync();
            }
            catch
            {
                return new List<BranchUserStatsModel>();
            }
        }

        public async Task<vmCheckExistBranchCode?> CheckExistBranchCode(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<vmCheckExistBranchCode>().FromSqlInterpolated($@"
                EXEC CheckExistBranchCode
                    @BranchCode = {filter.Title}
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<vmGetAllBranchesListByCompanyId>> GetAllBranchesListByCompanyId(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<vmGetAllBranchesListByCompanyId>().FromSqlInterpolated($@"
                EXEC GetAllBranchesListByCompanyId
                    @CompanyId = {filter.CompanyId}
                    
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<vmGetAllBranchesListByCompanyId>();
            }
        }

        public async Task<List<BranchViewModel>> GetBranchesByEmployee(int EmpId, int CompId)
        {
            try
            {
                var result = await _db.Set<BranchViewModel>().FromSqlInterpolated($@"
                EXEC sp_GetBranchesByEmployee
                    @LoginEmpId ={EmpId},
                    @CompanyId={CompId}
                    
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<BranchViewModel>();
            }
        }

        public async Task<List<EmployeeViewModel>> GetEmployeesByBranchAndUser(int EmpId, int CompId )
        {
            try
            {
                var result = await _db.Set<EmployeeViewModel>().FromSqlInterpolated($@"
                EXEC sp_GetEmployeesByBranchAndUser
                    @LoginEmpId = {EmpId},
                    @CompanyId ={CompId}
                 
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<EmployeeViewModel>();
            }
        }
    }
}
