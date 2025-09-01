using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class SalarySlipReport
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }  // int (not string)
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? PayableDays { get; set; }  // decimal (not int)
        public int? MonthNumber { get; set; }
        public string? MonthName { get; set; }
        public int? Year { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HRA { get; set; }
        public decimal? ConveyanceAllowance { get; set; }
        public decimal? ChildEducationAllowance { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? DeputationAllowance { get; set; }
        public decimal? TotalGrossSalary { get; set; }
        public decimal? PF { get; set; }
        public decimal? ESIC { get; set; }
        public decimal? ProfessionalTax { get; set; }
        public decimal? GroupMedical { get; set; }
        public decimal? TermInsurance { get; set; }
        public decimal? LWF { get; set; }
        public decimal? TDS { get; set; }
        public decimal? Loan { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? NetSalary { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? MonthDays { get; set; }
        public int? Holiday { get; set; }
        public decimal? AbsentDays { get; set; }  // decimal (not int)
        public decimal? PresentDays { get; set; }  // decimal (not int)
        public int? WeekOff { get; set; }
        public int? Leave { get; set; }
        public int? SalaryDays { get; set; }

        // Employee details from joins
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? PrimaryAccountNumber { get; set; }
        public string? PrimaryBankName { get; set; }
        public string? PANNo { get; set; }
        public string? PFNo { get; set; }
        public string? ESICNo { get; set; }
        public string? UANNumber { get; set; }
        public string? BranchName { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? GradeName { get; set; }

    }
}
