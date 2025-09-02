using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class EmployeeSalaryDaysViewModel
    {
        public string? Month { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public string? MonthName { get; set; }
        public int PresentDays { get; set; }
        public int HolidayDays { get; set; }
        public int LeaveDays { get; set; }
        public int WeekOffDays { get; set; }
        public int AbsentDays { get; set; }
        public int TotalDays { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
    } 
}
