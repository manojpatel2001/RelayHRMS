using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class GetLoanApplicationViewModel
    {
        public string? Employeecode { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public DateTime Applicationdate { get; set; }
        public DateTime LoanRequireDate { get; set; }
        public decimal LoanAmount { get; set; }
        public int NoOfinstallment { get; set; }
        public DateTime InstallmentstartDate { get; set; }
        public string? Remark { get; set; }
        public int LoanApplicationId { get; set; }
        public string? LoanName { get; set; }
    }
}
