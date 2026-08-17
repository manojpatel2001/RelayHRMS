namespace HRMS_Core.VM.ManagePermision
{
    // Read model for the "Company Access" grid — kept separate from
    // VMUserCompanyPermission (the create/update input model) because EF
    // Core's FromSqlInterpolated requires every property on the mapped type
    // to have a matching column in the query's result set.
    public class VMUserCompanyPermissionListItem
    {
        public int UserCompanyPermissionId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsEnabled { get; set; }

        // Comma-separated names of other employees who already have access
        // to this same company, for visibility while assigning.
        public string? OtherAssignedUsers { get; set; }
    }
}
