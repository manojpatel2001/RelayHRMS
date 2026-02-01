using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class GetExitApproval
    {
        public int ExitApplicationID { get; set; }
        public int Employeeid { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? LastWorkingDate { get; set; }
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string? UpdatedByName { get; set; } // Maps to Updated.FullName in the SP
        public DateTime? UpdatedDate { get; set; }
    }
}
