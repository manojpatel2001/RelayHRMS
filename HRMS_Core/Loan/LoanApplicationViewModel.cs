
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Loan
{
    public class LoanApplicationViewModel
    {
        public int LoanApplicationID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int EmployeeId { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public int LoanId { get; set; }
        public DateTime LoanRequireDate { get; set; }
        public decimal LoanMaxLimit { get; set; }
        public decimal LoanAmount { get; set; }
        public string InterestType { get; set; }
        public decimal InterestPercentage { get; set; }
        public int NoOfInstallment { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public string Remark { get; set; }
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
