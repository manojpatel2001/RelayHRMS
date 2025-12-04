using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class SalaryReportDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal PayableDays { get; set; }
        public int? MonthDays { get; set; }
        public int? Holiday { get; set; }
        public decimal? AbsentDays { get; set; }
        public decimal? PresentDays { get; set; }
        public int? WeekOff { get; set; }
        public decimal? Leave { get; set; }
        public decimal? SalaryDays { get; set; }

        public int MonthNumber { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal ChildEducationAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal DeputationAllowance { get; set; }
        public decimal TotalGrossSalary { get; set; }

        public decimal PF { get; set; }
        public decimal ESIC { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal GroupMedical { get; set; }
        public decimal TermInsurance { get; set; }
        public decimal LWF { get; set; }
        public decimal TDS { get; set; }
        public decimal Loan { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal Arrears { get; set; }
        public decimal OtherDeduction { get; set; }
        public decimal NetSalary { get; set; }
    }

}
