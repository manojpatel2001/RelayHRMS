using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class EmployeeDetailsloanViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime DateOfJoining { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MobileNo { get; set; }
        public string? OfficialEmail { get; set; }
        public string? PANNo { get; set; }
        public string? AadharCardNo { get; set; }
    }
}
