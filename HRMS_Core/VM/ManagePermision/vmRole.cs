namespace HRMS_Core.VM.ManagePermision
{
    public class vmRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public string Slug { get; set; }
        public string? AccessLevel { get; set; }
        public bool IsEnabled { get; set; } = true;

    }
}
