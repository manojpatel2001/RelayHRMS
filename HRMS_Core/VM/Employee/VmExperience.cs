using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class VmExperience
    {
        public int ExperienceId { get; set; }
        public int EmployeeId { get; set; }

        public string? Employer { get; set; }
        public string? Branch { get; set; }
        public string? Location { get; set; }
        public string? Designation { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? CTC { get; set; }
        public decimal? GrossSalary { get; set; }

        public string? Manager { get; set; }
        public string? ManagerContactNo { get; set; }

        public string? Remarks { get; set; }
        public string? IndustryType { get; set; }

        public string? DocumentPath { get; set; } // for uploaded file

        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}

