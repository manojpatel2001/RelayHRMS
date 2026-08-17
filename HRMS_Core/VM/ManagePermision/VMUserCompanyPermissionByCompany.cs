namespace HRMS_Core.VM.ManagePermision
{
    // Read model for the Control Panel "Company Access" grid — lists every
    // employee who already has access to a given company (the inverse
    // direction of VMUserCompanyPermissionListItem, which lists a given
    // employee's companies).
    public class VMUserCompanyPermissionByCompany
    {
        public int UserCompanyPermissionId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
