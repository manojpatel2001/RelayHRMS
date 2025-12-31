using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class LoanApplicationResult
    {
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string BranchName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LoanRequireDate { get; set; }
        public decimal LoanAmount { get; set; }
        public int NoOfInstallment { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public string Remark { get; set; }
        public int LoanApplicationId { get; set; }
        public string LoanName { get; set; }
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
    
}
}
