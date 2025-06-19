using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.PrivilegeSetting
{
    public class PanelHierarchyVM
    {
        public int PagePanelId { get; set; }
        public string? PagePanelName { get; set; }
        public List<PageMasterVM>? Pages { get; set; }
    }

    public class PageMasterVM
    {
        public int PageMasterId { get; set; }
        public int PrivilegeDetailsId { get; set; }
        public string? PageName { get; set; }
        public string? PageUrl { get; set; }
        public bool Is_Delete { get; set; }
        public bool Is_Edit { get; set; }
        public bool Is_View { get; set; }
        public bool Is_Save { get; set; }
        public List<SubPageMasterVM>? SubPages { get; set; }
    }

    public class SubPageMasterVM
    {
        public int PageMasterId { get; set; }
        public int PrivilegeDetailsId { get; set; }
        public string? PageName { get; set; }
        public string? PageUrl { get; set; }
        public bool Is_Delete { get; set; }
        public bool Is_Edit { get; set; }
        public bool Is_View { get; set; }
        public bool Is_Save { get; set; }
        public List<SubSubPageMasterVM>? SubSubPages { get; set; }

    }
    public class SubSubPageMasterVM
    {
        public int PageMasterId { get; set; }
        public string? PageName { get; set; }
        public string? PageUrl { get; set; }
        public bool Is_Delete { get; set; }
        public bool Is_Edit { get; set; }
        public bool Is_View { get; set; }
        public bool Is_Save { get; set; }
    }

}
