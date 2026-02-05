using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class EmployeeProfile_Skill
    {
        public int? EmployeeProfile_SkillId { get; set; }

        public string? SkillName { get; set; }
        public int? SkillMasterId { get; set; }
        public int? YearsOfExperience { get; set; }

        public string? Comments { get; set; }

        public int? EmployeeId { get; set; }
      

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

