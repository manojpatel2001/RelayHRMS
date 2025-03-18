using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("State")]
    public class State : BaseModel
    {
        [Key]
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? PTDeductionType { get; set; }  // Professional Tax Deduction Type (e.g., Monthly, Quarterly)
        public string? PTDeductionPeriod { get; set; }  // Professional Tax Deduction Period (e.g., Monthly, Quarterly)
        public string? EnrollmentCertificateNo { get; set; }  // Enrollment Certificate Number for Professional Tax
        public string? ESICStateCode { get; set; }  // ESIC State Code
        public string? ESICRegisteredOfficeAddress { get; set; }  // Address of the ESIC Registered Office
        public bool ApplicablePTSettingForMale_Female { get; set; } = false; // Applicable PT Settings for Male/Female  
    }

}
