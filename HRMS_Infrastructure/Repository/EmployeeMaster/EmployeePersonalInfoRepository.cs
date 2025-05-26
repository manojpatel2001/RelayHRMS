using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class EmployeePersonalInfoRepository : Repository<EmployeePersonalInfo>, IEmployeePersonalInfoRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeePersonalInfoRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<EmployeePersonalInfo>> GetAllEmployeePersonalInfo()
        {
            try
            {
                return await _db.Set<EmployeePersonalInfo>()
                    .FromSqlInterpolated($"EXEC GetAllEmployeePersonalInfoWithJoin")
                    .ToListAsync();
            }
            catch
            {
                return new List<EmployeePersonalInfo>();
            }
        }
        public async Task<EmployeePersonalInfo?> GetEmployeePersonalInfoByEmployeeId(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<EmployeePersonalInfo>()
                    .FromSqlInterpolated($"EXEC GetEmployeePersonalInfoByEmployeeId @EmployeeId= {EmployeeId}")
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public async Task<EmployeePersonalInfo?> GetEmployeePersonalInfoById(int employeePersonalInfoId)
        {
            try
            {
                var result = await _db.Set<EmployeePersonalInfo>()
                    .FromSqlInterpolated($"EXEC GetEmployeePersonalInfoById @EmployeePersonalInfoId = {employeePersonalInfoId}")
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateEmployeePersonalInfo(EmployeePersonalInfo employeePersonalInfo)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>()
                    .FromSqlInterpolated($@"
                    EXEC ManageEmployeePersonalInfo
                        @Action = {"CREATE"},
                        @Gender = {employeePersonalInfo.Gender},
                        @PersonalEmailId = {employeePersonalInfo.PersonalEmailId},
                        @FatherName = {employeePersonalInfo.FatherName},
                        @MotherName = {employeePersonalInfo.MotherName},
                        @DateOfBirth = {employeePersonalInfo.DateOfBirth},
                        @BloodGroup = {employeePersonalInfo.BloodGroup},
                        @Height = {employeePersonalInfo.Height},
                        @MaritalStatus = {employeePersonalInfo.MaritalStatus},
                        @MarriageDate = {employeePersonalInfo.MarriageDate},
                        @MarkIdentification = {employeePersonalInfo.MarkIdentification},
                        @Religion = {employeePersonalInfo.Religion},
                        @Caste = {employeePersonalInfo.Caste},
                        @CastCategory = {employeePersonalInfo.CastCategory},
                        @AadharCardNo = {employeePersonalInfo.AadharCardNo},
                        @PANNo = {employeePersonalInfo.PANNo},
                        @Dispensary = {employeePersonalInfo.Dispensary},
                        @DoctorName = {employeePersonalInfo.DoctorName},
                        @DispensaryAddress = {employeePersonalInfo.DispensaryAddress},
                        @UANNumber = {employeePersonalInfo.UANNumber},
                        @DrivingLicense = {employeePersonalInfo.DrivingLicense},
                        @DrivingLicenseExpiry = {employeePersonalInfo.DrivingLicenseExpiry},
                        @RationCardType = {employeePersonalInfo.RationCardType},
                        @RationCardNo = {employeePersonalInfo.RationCardNo},
                        @LinkedInId = {employeePersonalInfo.LinkedInId},
                        @TwitterId = {employeePersonalInfo.TwitterId},
                        @ProbationCompletionPeriod = {employeePersonalInfo.ProbationCompletionPeriod},
                        @ProbationPeriodType = {employeePersonalInfo.ProbationPeriodType},
                        @ManagerProbation = {employeePersonalInfo.ManagerProbation},
                        @ConfirmDate = {employeePersonalInfo.ConfirmDate},
                        @RetirementDate = {employeePersonalInfo.RetirementDate},
                        @OfferDate = {employeePersonalInfo.OfferDate},
                        @TraineeCompletionPeriod = {employeePersonalInfo.TraineeCompletionPeriod},
                        @TraineePeriodType = {employeePersonalInfo.TraineePeriodType},
                        @CanteenCode = {employeePersonalInfo.CanteenCode},
                        @TallyLedgerName = {employeePersonalInfo.TallyLedgerName},
                        @IsMetroCity = {employeePersonalInfo.IsMetroCity},
                        @AdultWorkerNo = {employeePersonalInfo.AdultWorkerNo},
                        @PhysicalDisability = {employeePersonalInfo.PhysicalDisability},
                        @MinimumWageSkillType = {employeePersonalInfo.MinimumWageSkillType},
                        @VehicleNo = {employeePersonalInfo.VehicleNo},
                        @InsuranceNo = {employeePersonalInfo.InsuranceNo},
                        @DressCode = {employeePersonalInfo.DressCode},
                        @ShirtSize = {employeePersonalInfo.ShirtSize},
                        @PantSize = {employeePersonalInfo.PantSize},
                        @ShoeSize = {employeePersonalInfo.ShoeSize},
                        @EmployeeId = {employeePersonalInfo.EmployeeId},
                        @IsDeleted = {employeePersonalInfo.IsDeleted},
                        @IsEnabled = {employeePersonalInfo.IsEnabled},
                        @IsBlocked = {employeePersonalInfo.IsBlocked},
                        @CreatedDate = {employeePersonalInfo.CreatedDate},
                        @CreatedBy = {employeePersonalInfo.CreatedBy},
                        @UpdatedDate = {employeePersonalInfo.UpdatedDate},
                        @UpdatedBy = {employeePersonalInfo.UpdatedBy},
                        @DeletedDate = {employeePersonalInfo.DeletedDate},
                        @DeletedBy = {employeePersonalInfo.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateEmployeePersonalInfo(EmployeePersonalInfo employeePersonalInfo)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>()
                    .FromSqlInterpolated($@"
                    EXEC ManageEmployeePersonalInfo
                        @Action = {"UPDATE"},
                        @EmployeePersonalInfoId = {employeePersonalInfo.EmployeePersonalInfoId},
                        @Gender = {employeePersonalInfo.Gender},
                        @PersonalEmailId = {employeePersonalInfo.PersonalEmailId},
                        @FatherName = {employeePersonalInfo.FatherName},
                        @MotherName = {employeePersonalInfo.MotherName},
                        @DateOfBirth = {employeePersonalInfo.DateOfBirth},
                        @BloodGroup = {employeePersonalInfo.BloodGroup},
                        @Height = {employeePersonalInfo.Height},
                        @MaritalStatus = {employeePersonalInfo.MaritalStatus},
                        @MarriageDate = {employeePersonalInfo.MarriageDate},
                        @MarkIdentification = {employeePersonalInfo.MarkIdentification},
                        @Religion = {employeePersonalInfo.Religion},
                        @Caste = {employeePersonalInfo.Caste},
                        @CastCategory = {employeePersonalInfo.CastCategory},
                        @AadharCardNo = {employeePersonalInfo.AadharCardNo},
                        @PANNo = {employeePersonalInfo.PANNo},
                        @Dispensary = {employeePersonalInfo.Dispensary},
                        @DoctorName = {employeePersonalInfo.DoctorName},
                        @DispensaryAddress = {employeePersonalInfo.DispensaryAddress},
                        @UANNumber = {employeePersonalInfo.UANNumber},
                        @DrivingLicense = {employeePersonalInfo.DrivingLicense},
                        @DrivingLicenseExpiry = {employeePersonalInfo.DrivingLicenseExpiry},
                        @RationCardType = {employeePersonalInfo.RationCardType},
                        @RationCardNo = {employeePersonalInfo.RationCardNo},
                        @LinkedInId = {employeePersonalInfo.LinkedInId},
                        @TwitterId = {employeePersonalInfo.TwitterId},
                        @ProbationCompletionPeriod = {employeePersonalInfo.ProbationCompletionPeriod},
                        @ProbationPeriodType = {employeePersonalInfo.ProbationPeriodType},
                        @ManagerProbation = {employeePersonalInfo.ManagerProbation},
                        @ConfirmDate = {employeePersonalInfo.ConfirmDate},
                        @RetirementDate = {employeePersonalInfo.RetirementDate},
                        @OfferDate = {employeePersonalInfo.OfferDate},
                        @TraineeCompletionPeriod = {employeePersonalInfo.TraineeCompletionPeriod},
                        @TraineePeriodType = {employeePersonalInfo.TraineePeriodType},
                        @CanteenCode = {employeePersonalInfo.CanteenCode},
                        @TallyLedgerName = {employeePersonalInfo.TallyLedgerName},
                        @IsMetroCity = {employeePersonalInfo.IsMetroCity},
                        @AdultWorkerNo = {employeePersonalInfo.AdultWorkerNo},
                        @PhysicalDisability = {employeePersonalInfo.PhysicalDisability},
                        @MinimumWageSkillType = {employeePersonalInfo.MinimumWageSkillType},
                        @VehicleNo = {employeePersonalInfo.VehicleNo},
                        @InsuranceNo = {employeePersonalInfo.InsuranceNo},
                        @DressCode = {employeePersonalInfo.DressCode},
                        @ShirtSize = {employeePersonalInfo.ShirtSize},
                        @PantSize = {employeePersonalInfo.PantSize},
                        @ShoeSize = {employeePersonalInfo.ShoeSize},
                        @EmployeeId = {employeePersonalInfo.EmployeeId},
                        @UpdatedDate = {employeePersonalInfo.UpdatedDate},
                        @UpdatedBy = {employeePersonalInfo.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteEmployeePersonalInfo(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>()
                    .FromSqlInterpolated($@"
                    EXEC ManageEmployeePersonalInfo
                        @Action = {"DELETE"},
                        @EmployeePersonalInfoId = {deleteRecordVM.Id},
                        @DeletedDate = {deleteRecordVM.DeletedDate},
                        @DeletedBy = {deleteRecordVM.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        
    }
}
