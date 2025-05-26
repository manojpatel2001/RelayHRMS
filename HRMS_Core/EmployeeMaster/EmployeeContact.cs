using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    [Table("EmployeeContact")]
    public class EmployeeContact:BaseModel
    {
        [Key]
        public int EmployeeContactId { get; set; }
        // Present Address Details
        public string? PresentAddress { get; set; }
        public string? PresentTehsil { get; set; }
        public string? PresentDistrict { get; set; }
        public string? PresentCity { get; set; }
        public string? PresentState { get; set; }
        public string? PresentPincode { get; set; }
        public int? PresentThanaId { get; set; }
        [ForeignKey(nameof(PresentThanaId))]
        [ValidateNever]
        public Thana? PresentThana { get; set; }

        // Permanent Address Details
        public string? PermanentAddress { get; set; }
        public string? PermanentTehsil { get; set; }
        public string? PermanentDistrict { get; set; }
        public string? PermanentCity { get; set; }
        public string? PermanentState { get; set; }
        public string? PermanentPincode { get; set; }
        public int? PermanentThanaId { get; set; }
        [ForeignKey(nameof(PermanentThanaId))]
        [ValidateNever]
        public Thana? PermanentThana { get; set; }
        // Contact Information
        public int? CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        [ValidateNever]
        public Country? Country { get; set; }
        public string? WorkPhone { get; set; }
        public string? PersonalPhone { get; set; }
        public string? OfficialEmail { get; set; }
        public string? Nationality { get; set; }
        public string? ExtensionNo { get; set; }
        public string? MobileNo { get; set; }

        // Checkbox for same as present address
        public bool? SameAsPresentAddress { get; set; } = false;

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [ValidateNever]
        public Employee? Employee { get; set; }
    }

}
