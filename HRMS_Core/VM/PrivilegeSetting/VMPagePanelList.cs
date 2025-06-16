using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.PrivilegeSetting
{
    public class VMPagePanelList
    {
        public int PagePanelId { get; set; }
        public string? PagePanelName { get; set; }
    }
    public class VMPageMasterList
    {
        public int PageMasterId { get; set; }
        public string? PageName { get; set; }
        public int? UnderPageMasterId { get; set; }
        public string? UnderPageMasterName { get; set; }
    }
    
}
