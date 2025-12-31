using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class LoanApprovalSearchViewModel
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string? SearchType { get; set; } // 'LoanName', 'LoanStatus'
        public string? SearchFor { get; set; }  // e.g., 'Home Loan'
        public string? LoanStatus { get; set; }
    }
}
