using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.PrivilegeSetting
{
    public class vmPageMaster:BaseModel
    {
        public int PageMasterId { get; set; } = 0;
        public string? PageName { get; set; }
        public string? AliasPageName { get; set; }
        public int? UnderPageMasterId { get; set; }
        public string? UnderPageMasterName { get; set; }
        public string? PageUrl { get; set; }
        public int? SortId { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? ModuleDetailsId { get; set; }
        
        public int? PagePanelId { get; set; }
        public string? PagePanelName { get; set; }
    }
}
