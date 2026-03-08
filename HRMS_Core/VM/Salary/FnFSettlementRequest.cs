using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class FnFSettlementRequest
    {
        public string Action { get; set; } = "SaveFnF";
        public int? Id { get; set; }

        // Employee
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }   // nullable
        public string? EmployeeName { get; set; }   // nullable
        public int? CompanyId { get; set; }

        // Period
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MonthNumber { get; set; }
        public string? MonthName { get; set; }      // nullable — UI sends string
        public int Year { get; set; }

        // Attendance
        public int MonthDays { get; set; }
        public decimal PresentDays { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal Leave { get; set; }
        public int WeekOff { get; set; }
        public int Holiday { get; set; }
        public decimal HalfDays { get; set; }
        public decimal LWPDays { get; set; }
        public decimal PayableDays { get; set; }
        public int ArrearDays { get; set; }

        // Earnings
        public decimal GrossSalary { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal ChildEducationAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal DeputationAllowance { get; set; }
        public decimal Arrears { get; set; }
        public decimal TotalGrossSalary { get; set; }

        // Deductions
        public decimal PF { get; set; }
        public decimal ESIC { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal GroupMedical { get; set; }
        public decimal TermInsurance { get; set; }
        public decimal LWF { get; set; }
        public decimal TDS { get; set; }
        public decimal Loan { get; set; }
        public decimal OtherDeduction { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }

        // FnF Extras
        public decimal GratuityAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal TDSAmount { get; set; }
        public decimal OtherDeductionExtra { get; set; }
        public decimal LeaveEncashDays { get; set; }
        public string? ArrearMonth { get; set; }    // nullable — UI sends null
        public int? ArrearYear { get; set; }
        public string? Remarks { get; set; }        // nullable — UI sends null
        public int? CreatedBy { get; set; }
    }

}
