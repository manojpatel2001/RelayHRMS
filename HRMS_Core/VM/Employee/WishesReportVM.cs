using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class WishesReportVM
    {

        
            public int? Id { get; set; }
            public string? FullName { get; set; }
            public string? EmployeeProfileUrl { get; set; }
            public string? CompanyName { get; set; }
            public string? BranchName { get; set; }
            public string? DesignationName { get; set; }
            public string? DepartmentName { get; set; }
            public DateTime? Date { get; set; } 
            public int? AgeInYears { get; set; }
        

    }
}
