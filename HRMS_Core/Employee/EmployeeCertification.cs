using System;

namespace HRMS_Core.Employee
{
    public class EmployeeCertification
    {
        public int EmployeeCertificationId { get; set; }
        public int EmployeeId { get; set; }
        public string CertificationName { get; set; } = string.Empty;
        public string? IssuingBody { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CertificateUrl { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
