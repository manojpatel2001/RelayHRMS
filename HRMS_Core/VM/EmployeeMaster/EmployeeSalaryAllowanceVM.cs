using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class EmployeeSalaryAllowanceVM
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }

        public decimal? BasicSalary { get; set; }
        public decimal? ChildEducationAllowance { get; set; }
        public decimal? ConveyanceAllowance { get; set; }
        public decimal? HRA { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? DeputationAllowance { get; set; }
        public decimal? TotalGrossSalary { get; set; }

        public decimal? EmployeePF { get; set; }
        public decimal? EmployeeESI { get; set; }
        public decimal? ProfessionalTax { get; set; }
        public decimal? GroupMedical { get; set; }
        public decimal? TermInsurance { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? NetSalary { get; set; }

        public decimal? CTC { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

}
