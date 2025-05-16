using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.CompanyInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HRMS_Infrastructure.Repository.CompanyInformation
{
  
    public class CompanyDetailsRepository : Repository<CompanyDetails>, ICompanyDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public CompanyDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<vmGetAllCompanyDetails>> GetAllCompanyDetails()
        {
            try
            {
                return await _db.Set<vmGetAllCompanyDetails>().FromSqlInterpolated($"EXEC GetAllCompanyDetails").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllCompanyDetails>();
            }
        }

        public async Task<vmGetAllCompanyDetails?> GetByCompanyId(int companyId)
        {
            try
            {
                var result = await _db.Set<vmGetAllCompanyDetails>()
                    .FromSqlInterpolated($"EXEC GetByCompanyDetailsId @CompanyId = {companyId}").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateCompanyDetails(CompanyDetails companyDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCompanyDetails
                        @Action = {"CREATE"},
                        @CompanyName = {companyDetails.CompanyName},
                        @CompanyAddress = {companyDetails.CompanyAddress},
                        @CityId = {companyDetails.CityId},
                        @StateId = {companyDetails.StateId},
                        @PinCode = {companyDetails.PinCode},
                        @Country = {companyDetails.Country},
                        @PhoneNo = {companyDetails.PhoneNo},
                        @EmailAddress = {companyDetails.EmailAddress},
                        @DateFormat = {companyDetails.DateFormat},
                        @FromDate = {companyDetails.FromDate},
                        @Website = {companyDetails.Website},
                        @PfTrustNo = {companyDetails.PfTrustNo},
                        @PfApplicable = {companyDetails.PfApplicable},
                        @PFNo = {companyDetails.PFNo},
                        @EsicApplicable = {companyDetails.EsicApplicable},
                        @ESICNo = {companyDetails.ESICNo},
                        @TanNo = {companyDetails.TanNo},
                        @PanNo = {companyDetails.PanNo},
                        @DomainName = {companyDetails.DomainName},
                        @CompanyCode = {companyDetails.CompanyCode},
                        @LwfNo = {companyDetails.LwfNo},
                        @InOutDuration = {companyDetails.InOutDuration},
                        @HierarchyDesignation = {companyDetails.HierarchyDesignation},
                        @EmployeeLicense = {companyDetails.EmployeeLicense},
                        @EmailSignature = {companyDetails.EmailSignature},
                        @ContractorCompany = {companyDetails.ContractorCompany},
                        @IsDigitalSignature = {companyDetails.IsDigitalSignature},
                        @SelectWeekOffDay = {companyDetails.SelectWeekOffDay},
                        @AlternateWeekOffDay = {companyDetails.AlternateWeekOffDay},
                        @AlternateFullWeekOff = {companyDetails.AlternateFullWeekOff},
                        @CompanyLogoUrl = {companyDetails.CompanyLogoUrl},
                        @DigitalSignatureUrl = {companyDetails.DigitalSignatureUrl},
                        @DigitalSignaturePassword = {companyDetails.DigitalSignaturePassword},
                        @IsDisplayOnLogin = {companyDetails.IsDisplayOnLogin},
                        @LetterHeadFooterUrl = {companyDetails.LetterHeadFooterUrl},
                        @LetterHeadHeaderUrl = {companyDetails.LetterHeadHeaderUrl},
                        @IsDeleted = {companyDetails.IsDeleted},
                        @IsEnabled = {companyDetails.IsEnabled},
                        @IsBlocked = {companyDetails.IsBlocked},
                        @CreatedDate = {companyDetails.CreatedDate},
                        @CreatedBy = {companyDetails.CreatedBy},
                        @UpdatedDate = {companyDetails.UpdatedDate},
                        @UpdatedBy = {companyDetails.UpdatedBy},
                        @DeletedDate = {companyDetails.DeletedDate},
                        @DeletedBy = {companyDetails.DeletedBy},
                        @DigitsForEmployeeCode = {companyDetails.DigitsForEmployeeCode},
                        @MaxEmployeeCode = {companyDetails.MaxEmployeeCode},
                        @AlphaNumericCode = {companyDetails.AlphaNumericCode},
                        @SampleCode = {companyDetails.SampleCode},
                        @HrManager = {companyDetails.HrManager},
                        @HrManagerDesignation = {companyDetails.HrManagerDesignation},
                        @NatureOfBusiness = {companyDetails.NatureOfBusiness},
                        @DateOfFactorySetup = {companyDetails.DateOfFactorySetup},
                        @FactoryType = {companyDetails.FactoryType},
                        @FactoryLicenseOffice = {companyDetails.FactoryLicenseOffice},
                        @FactoryRegistrationNo = {companyDetails.FactoryRegistrationNo},
                        @FactoryLicenseNo = {companyDetails.FactoryLicenseNo},
                        @TdsDeductor = {companyDetails.TdsDeductor},
                        @FatherName = {companyDetails.FatherName},
                        @ManagerDesignation = {companyDetails.ManagerDesignation},
                        @CitAddress = {companyDetails.CitAddress},
                        @CitCity = {companyDetails.CitCity},
                        @CitPin = {companyDetails.CitPin},
                        @IssueDate = {companyDetails.IssueDate},
                        @GstTravelExpenses = {companyDetails.GstTravelExpenses}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateCompanyDetails(CompanyDetails companyDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCompanyDetails
                        @Action = {"UPDATE"},
                        @CompanyId = {companyDetails.CompanyId},
                        @CompanyName = {companyDetails.CompanyName},
                        @CompanyAddress = {companyDetails.CompanyAddress},
                        @CityId = {companyDetails.CityId},
                        @StateId = {companyDetails.StateId},
                        @PinCode = {companyDetails.PinCode},
                        @Country = {companyDetails.Country},
                        @PhoneNo = {companyDetails.PhoneNo},
                        @EmailAddress = {companyDetails.EmailAddress},
                        @DateFormat = {companyDetails.DateFormat},
                        @FromDate = {companyDetails.FromDate},
                        @Website = {companyDetails.Website},
                        @PfTrustNo = {companyDetails.PfTrustNo},
                        @PfApplicable = {companyDetails.PfApplicable},
                        @PFNo = {companyDetails.PFNo},
                        @EsicApplicable = {companyDetails.EsicApplicable},
                        @ESICNo = {companyDetails.ESICNo},
                        @TanNo = {companyDetails.TanNo},
                        @PanNo = {companyDetails.PanNo},
                        @DomainName = {companyDetails.DomainName},
                        @CompanyCode = {companyDetails.CompanyCode},
                        @LwfNo = {companyDetails.LwfNo},
                        @InOutDuration = {companyDetails.InOutDuration},
                        @HierarchyDesignation = {companyDetails.HierarchyDesignation},
                        @EmployeeLicense = {companyDetails.EmployeeLicense},
                        @EmailSignature = {companyDetails.EmailSignature},
                        @ContractorCompany = {companyDetails.ContractorCompany},
                        @IsDigitalSignature = {companyDetails.IsDigitalSignature},
                        @SelectWeekOffDay = {companyDetails.SelectWeekOffDay},
                        @AlternateWeekOffDay = {companyDetails.AlternateWeekOffDay},
                        @AlternateFullWeekOff = {companyDetails.AlternateFullWeekOff},
                        @UpdatedDate = {companyDetails.UpdatedDate},
                        @UpdatedBy = {companyDetails.UpdatedBy},
                        @DigitsForEmployeeCode = {companyDetails.DigitsForEmployeeCode},
                        @MaxEmployeeCode = {companyDetails.MaxEmployeeCode},
                        @AlphaNumericCode = {companyDetails.AlphaNumericCode},
                        @SampleCode = {companyDetails.SampleCode},
                        @HrManager = {companyDetails.HrManager},
                        @HrManagerDesignation = {companyDetails.HrManagerDesignation},
                        @NatureOfBusiness = {companyDetails.NatureOfBusiness},
                        @DateOfFactorySetup = {companyDetails.DateOfFactorySetup},
                        @FactoryType = {companyDetails.FactoryType},
                        @FactoryLicenseOffice = {companyDetails.FactoryLicenseOffice},
                        @FactoryRegistrationNo = {companyDetails.FactoryRegistrationNo},
                        @FactoryLicenseNo = {companyDetails.FactoryLicenseNo},
                        @TdsDeductor = {companyDetails.TdsDeductor},
                        @FatherName = {companyDetails.FatherName},
                        @ManagerDesignation = {companyDetails.ManagerDesignation},
                        @CitAddress = {companyDetails.CitAddress},
                        @CitCity = {companyDetails.CitCity},
                        @CitPin = {companyDetails.CitPin},
                        @IssueDate = {companyDetails.IssueDate},
                        @GstTravelExpenses = {companyDetails.GstTravelExpenses}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteCompanyDetails(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageCompanyDetails
                    @Action = {"DELETE"},
                    @CompanyId = {deleteRecordVM.Id},
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

        public async Task<VMCommonResult> UpdateCompanyLogo(vmChangeCompanyLogo vmChangeCompanyLogo)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC UpdateCompanyLogo
                    @CompanyId = {vmChangeCompanyLogo.companyId},
                    @IsDisplayOnLogin = {vmChangeCompanyLogo.IsDisplayOnLogin},
                    @CompanyLogoUrl = {vmChangeCompanyLogo.CompanyLogoUrl}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateLetterHead(vmUploadHeader vmUploadHeader)
        {
            try
            {
              
                // Call the stored procedure
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateLetterHead
                        @CompanyId = {vmUploadHeader.companyId},
                        @CompanyName = {vmUploadHeader.CompanyName},
                        @CompanyAddress = {vmUploadHeader.CompanyAddress},
                        @LetterHeadHeaderUrl = {vmUploadHeader.LetterHeadHeaderUrl},
                        @LetterHeadFooterUrl = {vmUploadHeader.LetterHeadFooterUrl},
                        @EffectiveDate = {vmUploadHeader.EffectiveDate}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateDigitalSignature(vmDigitalSignature vmDigitalSignature)
        {
            try
            {
                // Call the stored procedure
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateDigitalSignature
                        @CompanyId = {vmDigitalSignature.CompanyId},
                        @IsDigitalSignature = {vmDigitalSignature.IsDigitalSignature},
                        @DigitalSignatureUrl = {vmDigitalSignature.DigitalSignatureUrl},
                        @DigitalSignaturePassword = {vmDigitalSignature.DigitalSignaturePassword}
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
