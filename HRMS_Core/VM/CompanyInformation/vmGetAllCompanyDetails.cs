using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyInformation
{

    public class vmGetAllCompanyDetails
    {
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public string? PinCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? DateFormat { get; set; }
        public DateTime? FromDate { get; set; }
        public string? Website { get; set; }
        public string? PfTrustNo { get; set; }
        public bool? PfApplicable { get; set; }
        public string? PFNo { get; set; }
        public bool? EsicApplicable { get; set; }
        public string? ESICNo { get; set; }
        public string? TanNo { get; set; }
        public string? PanNo { get; set; }
        public string? DomainName { get; set; }
        public string? CompanyCode { get; set; }
        public string? LwfNo { get; set; }
        public string? EmployeeCodeSetting { get; set; }
        public int? InOutDuration { get; set; }
        public bool? HierarchyDesignation { get; set; }
        public string? EmployeeLicense { get; set; }
        public string? EmailSignature { get; set; }
        public bool? ContractorCompany { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public string? SelectWeekOffDay { get; set; }
        public string? AlternateWeekOffDay { get; set; }
        public string? AlternateFullWeekOff { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public string? DigitalSignatureUrl { get; set; }
        public string? DigitalSignaturePassword { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? LetterHeadHeaderUrl { get; set; }
        public string? LetterHeadFooterUrl { get; set; }
        public bool? IsDisplayOnLogin { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsBlocked { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

}
