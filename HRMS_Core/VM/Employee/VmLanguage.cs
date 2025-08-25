using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class VmLanguage
    {
        public int LanguageId { get; set; }
        public int? EmployeeId { get; set; }

        public string? LanguageName { get; set; }   // English, Hindi etc.
        public string? Fluency { get; set; }        // Beginner, Intermediate, Expert

        // Ability (checkboxes) nullable banaye
        public bool? CanWrite { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanSpeak { get; set; }
        public bool? CanUnderstand { get; set; }

        // Audit Fields
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

    }
}
