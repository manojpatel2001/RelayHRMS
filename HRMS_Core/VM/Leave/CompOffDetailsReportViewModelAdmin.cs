using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class CompOffDetailsReportViewModelAdmin
    {
        public int? Comp_Off_Detailsid { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public string? DesignationName { get; set; }
        public DateTime? ExtraWorkDay { get; set; }
        public string? ExtraWorkHours { get; set; }
        public string? ComoffReason { get; set; }
        public string? ApplicationStatus { get; set; }

    }
}
