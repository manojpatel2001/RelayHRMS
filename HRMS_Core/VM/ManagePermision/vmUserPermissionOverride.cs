namespace HRMS_Core.VM.ManagePermision
{
    public class vmUserPermissionOverride
    {
        public int EmployeeId { get; set; }
        public string PermissionIds { get; set; }
        public int CompanyId { get; set; }
        public bool IsGranted { get; set; }
    }
}
