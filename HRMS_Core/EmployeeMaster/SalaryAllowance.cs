using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    public class SalaryAllowance:BaseModel
    {
        public int SalaryAllowanceId {  get; set; }
        public int? EmployeeId {  get; set; }
        public int? CompanyId {  get; set; }
        public double? BasicSalaryPercentage {  get; set; }
        public double? ChildEducationAllowance {  get; set; }
        public double? HRA {  get; set; }
        public double? MedicalAllowance {  get; set; }
        public double? TotalGrossSalary {  get; set; }
        public double? PF {  get; set; }
        public double? ESI {  get; set; }
        public double? ProfessionalTax {  get; set; }
        public double? GroupMedicalPercentage {  get; set; }
        public double? TermEnsurancePercentage {  get; set; }
        public double? TotalDeduction {  get; set; }
        public double? NetSalary {  get; set; }
        public double? CTC {  get; set; }

    }
}
