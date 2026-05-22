using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class MonthlySalaryRequestViewModel
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? EmployeeCodes { get; set; }
        public string? BranchId { get; set; }
        public int? CompanyId { get; set; }
        public string? Action { get; set; }
        public int CreatedBy { get; set; }

    }

    public class SalaryDetailsParameterVm
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string? EmployeeCodes { get; set; }
        public int? BranchId { get; set; }
    

    }
    public class GetEmployeePayableDaysRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeIds { get; set; } // Single Emp_Id (or comma-separated if needed)
    }
    public class EmployeePayableDaysResponse
    {
        public string BranchName { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int Emp_Id { get; set; }
        public decimal TotalPayableDays { get; set; }
        public int TotalMonthDays { get; set; }
    }
}
    
