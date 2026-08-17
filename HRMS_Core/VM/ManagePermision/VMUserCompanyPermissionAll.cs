namespace HRMS_Core.VM.ManagePermision
{
    // Read model for the flat, system-wide "Company Access" grid — every
    // employee-company assignment across every company, with a per-employee
    // company count so users with access to more than one company stand out.
    public class VMUserCompanyPermissionAll
    {
        public int UserCompanyPermissionId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsEnabled { get; set; }
        public int TotalCompanies { get; set; }
    }
}
