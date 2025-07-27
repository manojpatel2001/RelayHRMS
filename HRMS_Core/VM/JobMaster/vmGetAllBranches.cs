using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.JobMaster
{
    public class vmGetAllBranches
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public string? GSTIN_No { get; set; }
        public bool? IsActive { get; set; } 

    }
}
