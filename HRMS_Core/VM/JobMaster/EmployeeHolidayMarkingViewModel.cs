using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.JobMaster
{
    public class EmployeeHolidayMarkingViewModel
    {
        public int EmployeeHolidayId { get; set; }
        public string? FullName { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BranchName { get; set; }
        public int BranchId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string? HolidayName { get; set; }
        public string? Marked { get; set; }
    }
}
