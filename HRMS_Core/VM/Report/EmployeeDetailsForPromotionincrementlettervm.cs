using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class EmployeeDetailsForPromotionincrementlettervm
    {
        public string? FullName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? Gender { get; set; }
        public string? EmployeeCode { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyName { get; set; }
        public Decimal? NewGrossSalary { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public string? RefNumber { get; set; }
    }
}
