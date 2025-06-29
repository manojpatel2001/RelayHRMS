namespace HRMS_Core.VM.ManagePermision
{
    public class RoleManagePermissionDto
    {
        public int? RoleId { set; get; }
        public string? RoleName { set; get; }
        public string? Slug { set; get; }
        public List<ManagePermissionDto2> ?Permissions { set; get; }
    }

    public class ManagePermissionDto2
    {
        public int? PermissionId { set; get; }
        public string? PermissionName { set; get; }
        public string? Slug { set; get; }
    }

}
