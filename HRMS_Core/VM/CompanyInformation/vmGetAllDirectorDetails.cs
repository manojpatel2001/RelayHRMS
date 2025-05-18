using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyInformation
{
    public class vmGetAllDirectorDetails
    {
        public int? DirectorDetailsId { get; set; }
        public string? DirectorName { get; set; }
        public string? DirectorAddress { get; set; }
        public DateTime? DirectorDOB { get; set; }
        public string? DirectorBranch { get; set; }
        public string? DirectorDesignation { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; } 
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
