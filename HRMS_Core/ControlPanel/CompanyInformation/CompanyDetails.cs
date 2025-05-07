using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        [ForeignKey(nameof(CityId))]
        [ValidateNever]
        public City? City { get; set; }
        public int? StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        [ValidateNever]
        public State? State { get; set; }
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
        public string? EmployeeCodeSetting { get; set; }
        public int? InOutDuration { get; set; }
        public bool? HierarchyDesignation { get; set; }
        public string? EmployeeLicense { get; set; }
        public string? EmailSignature { get; set; }
        public bool? ContractorCompany { get; set; }
        public string? DigitalSignature { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public string? SelectWeekOffDay { get; set; }
        public string? AlternateWeekOffDay { get; set; }
        public string? AlternateFullWeekOff { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public string? DigitalSignatureUrl { get; set; }
        public string? DigitalSignaturePassword { get; set; }

        
    }

}
