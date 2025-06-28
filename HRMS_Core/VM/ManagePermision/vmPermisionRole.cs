using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ManagePermision
{
    public class vmPermisionRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public string Slug { get; set; }
        public  List<int> PermissionIds { get; set; }
    }
}
