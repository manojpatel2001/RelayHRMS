using HRMS_Core.DbContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("Branch")]
    public class Branch : BaseModel
    {
        [Key]
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }

        public int? CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public City? City { get; set; }

        public DateTime SalaryStartDate { get; set; }
        public bool ContractorBranch { get; set; } =false;

        public string? RegistrationCertificateNo { get; set; }
        public string? Zone { get; set; }
        public string? WardNumber { get; set; }
        public string? CensusNumber { get; set; }
        public string? PFNo { get; set; }
        public string? ESICNo { get; set; }
    }
}
