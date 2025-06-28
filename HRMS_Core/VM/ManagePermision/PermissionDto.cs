using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ManagePermision
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Slug { get; set; }
    }

    public class GroupedPermissionDto
    {
        public string GroupName { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

}
