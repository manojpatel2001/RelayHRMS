using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.JobMaster;
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

        public async Task<bool> UpdateBranch(Branch branch)
        {
            var existingRecord = await _db.Branch.SingleOrDefaultAsync(asd => asd.BranchId == branch.BranchId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.BranchName = branch.BranchName;
            existingRecord.BranchCode = branch.BranchCode;
            existingRecord.CompanyName = branch.CompanyName;
            existingRecord.Address = branch.Address;
            //existingRecord.CityId = branch.CityId;
            existingRecord.CityName = branch.CityName;
            existingRecord.CountryName = branch.CountryName;
            existingRecord.State = branch.State;    
            existingRecord.SalaryStartDate = branch.SalaryStartDate;
            existingRecord.ContractorBranch = branch.ContractorBranch;
            existingRecord.RegistrationCertificateNo = branch.RegistrationCertificateNo;
            existingRecord.Zone = branch.Zone;
            existingRecord.WardNumber = branch.WardNumber;
            existingRecord.CensusNumber = branch.CensusNumber;
            existingRecord.PFNo = branch.PFNo;
            existingRecord.ESICNo = branch.ESICNo;
            existingRecord.UpdatedBy = branch.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        public async Task<Branch> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var branch = await _db.Branch.FirstOrDefaultAsync(asd => asd.BranchId == DeleteRecord.Id);
            if (branch == null)
            {
                return branch;
            }
            else
            {
                branch.IsEnabled = false;
                branch.IsDeleted = true;
                branch.DeletedDate = DateTime.UtcNow;
                branch.DeletedBy = DeleteRecord.DeletedBy;
                return branch;
            }
        }

        public async Task<List<BranchUserStatsModel>> GetBranchWiseEmpCount()
        {
            try
            {
                return await _db.Set<BranchUserStatsModel>().FromSqlInterpolated($"EXEC GetBranchUserStats").ToListAsync();
            }
            catch
            {
                return new List<BranchUserStatsModel>();
            }
        }
    }
}
