using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM
{
    public class PageVM
    {
        public int? CompanyId { get; set; }
        public int? PagePanelId { get; set; }
        public int? PrivilegeMasterId { get; set; }
    }
}
