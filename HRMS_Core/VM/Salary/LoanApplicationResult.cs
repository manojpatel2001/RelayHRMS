using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class LoanApplicationResult
    {
        public int LoanApplicationId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BranchName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LoanRequireDate { get; set; }
        public decimal LoanAmount { get; set; }
        public int NumberOfInstallments { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public string Reason { get; set; }
        public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; }
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string LoanStatus { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
