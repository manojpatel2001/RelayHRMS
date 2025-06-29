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
       
        public  List<int> PermissionIds { get; set; }
        public  int? CompanyId { get; set; }
    }
}
