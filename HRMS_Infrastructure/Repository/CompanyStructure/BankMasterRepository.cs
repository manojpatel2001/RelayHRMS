using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.CompanyStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.CompanyStructure
{
    public class BankMasterRepository:Repository<BankMaster>,IBankMasterRepository
    {
        private HRMSDbContext _db;

        public BankMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateBankMaster(BankMaster bankMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC CreateBankMaster 
                    @BankName = {bankMaster.BankName},
                    @BankCode = {bankMaster.BankCode},
                    @BranchName = {bankMaster.BranchName},
                    @AccountNo = {bankMaster.AccountNo},
                    @Address = {bankMaster.Address},
                    @City = {bankMaster.City},
                    @BankBSRCode = {bankMaster.BankBSRCode},
                    @IsDefaultBank = {bankMaster.IsDefaultBank},
                    @IsDeleted = {bankMaster.IsDeleted},
                    @IsEnabled = {bankMaster.IsEnabled},
                    @IsBlocked = {bankMaster.IsBlocked},
                    @CreatedDate = {bankMaster.CreatedDate},
                    @CreatedBy = {bankMaster.CreatedBy},
                    @DeletedDate = {bankMaster.DeletedDate},
                    @DeletedBy = {bankMaster.DeletedBy},
                    @UpdatedDate = {bankMaster.UpdatedDate},
                    @UpdatedBy = {bankMaster.UpdatedBy}
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

        public async Task<VMCommonResult> DeleteBankMaster(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC DeleteBankMaster 
                    @BankMasterId = {deleteRecordVM.Id},
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

        public async Task<VMCommonResult> UpdateBankMaster(BankMaster bankMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC UpdateBankMaster 
                    @BankMasterId = {bankMaster.BankMasterId},
                    @BankName = {bankMaster.BankName},
                    @BankCode = {bankMaster.BankCode},
                    @BranchName = {bankMaster.BranchName},
                    @AccountNo = {bankMaster.AccountNo},
                    @Address = {bankMaster.Address},
                    @City = {bankMaster.City},
                    @BankBSRCode = {bankMaster.BankBSRCode},
                    @IsDefaultBank = {bankMaster.IsDefaultBank},
                    @UpdatedDate = {bankMaster.UpdatedDate},
                    @UpdatedBy = {bankMaster.UpdatedBy}
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
