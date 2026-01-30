using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class AttendanceRegularizationSearchFilterForAdminVM
    {

        public string? SearchBy { get; set; }       
        public string? SearchValue { get; set; }    
        public DateTime? FromDate { get; set; }   
        public DateTime? ToDate { get; set; }     
        public string? Status { get; set; } 
        public int? CompanyId { get; set; } 
        public int? EmployeeId { get; set; } 
    }
}
