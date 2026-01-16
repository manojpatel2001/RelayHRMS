using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class LoanApprovalSearchViewModel
    {
        public string? LoanType { get; set; }      // Loan Name for filtering
        public string? Status { get; set; }        // 'isPending' or 'isApprovedReject'
        public string? SearchFor { get; set; }     // Employee Code or Employee Name
        public int Emplooyeid { get; set; }        // Employee ID
        public int CompId { get; set; }
    }
}
