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
        public string? Address { get; set; }
        public int? CityId{ get; set; }
        public string? CountryName { get; set; }   
        public int? StateId { get; set; }   
        public string? GSTIN_No { get; set; }
        public bool? IsActive { get; set; } = false;

    }
}
