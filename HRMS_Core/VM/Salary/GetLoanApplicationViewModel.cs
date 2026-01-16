using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class GetLoanApplicationViewModel
    {
        public string EmployeeCode { get; set; }
        public int id { get; set; }
        public int LoanId { get; set; }
        public decimal InstallmentAmount { get; set; }
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
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
    }
}
