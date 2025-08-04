using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.ControlPanel.CompanyInformation
{
    [Table("CompanyDetails")]
    public class CompanyDetails:BaseModel
    {
        [Key]
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public string? PinCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? DateFormat { get; set; }
        public DateTime? FromDate { get; set; }
        public string? Website { get; set; }
        public string? PfTrustNo { get; set; }
        public bool? PfApplicable { get; set; } = false;
        public string? PFNo { get; set; }
        public bool? EsicApplicable { get; set; } = false;
        public string? ESICNo { get; set; }
        public string? TanNo { get; set; }
        public string? PanNo { get; set; }
        public string? DomainName { get; set; }
        public string? CompanyCode { get; set; }
        public string? LwfNo { get; set; }
        public int? InOutDuration { get; set; }
        public bool? HierarchyDesignation { get; set; }
        public string? EmployeeLicense { get; set; }
        public string? EmailSignature { get; set; }
        public bool? ContractorCompany { get; set; }
        public string? SelectWeekOffDay { get; set; }
        public string? AlternateWeekOffDay { get; set; }
        public string? AlternateFullWeekOff { get; set; }
        public string? BranchIds { get; set; }
        //change logo
        public string? CompanyLogoUrl { get; set; }
        public bool? IsDisplayOnLogin { get; set; }
        //Upload header
        public DateTime? EffectiveDate { get; set; }
        public string? LetterHeadHeaderUrl { get; set; }
        public string? LetterHeadFooterUrl { get; set; }

        // upload signature
        public bool? IsDigitalSignature { get; set; }
        public string? DigitalSignatureUrl { get; set; }
        public string? DigitalSignaturePassword { get; set; }

        //employee code setting
        public string? DigitsForEmployeeCode { get; set; }
        public string? MaxEmployeeCode { get; set; } 
        public bool? AlphaNumericCode { get; set; }
        public string? SampleCode { get; set; }


        //other details
        public string? HrManager { get; set; }
        public string? HrManagerDesignation { get; set; }
        public string? NatureOfBusiness { get; set; }
        public DateTime? DateOfFactorySetup { get; set; } 
        public string? FactoryType { get; set; }
        public string? FactoryLicenseOffice { get; set; }
        public string? FactoryRegistrationNo { get; set; }
        public string? FactoryLicenseNo { get; set; }
        public string? TdsDeductor { get; set; }
        public string? FatherName { get; set; }
        public string? ManagerDesignation { get; set; }
        public string?CitAddress { get; set; }
        public string? CitCity { get; set; }
        public string? CitPin { get; set; } 
        public DateTime? IssueDate { get; set; } 
        public bool? GstTravelExpenses { get; set; } 
    }

}
