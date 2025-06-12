using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.PrivilegeSetting
{
    [Table("PageMaster")]
    public class PageMaster:BaseModel
    {
        [Key]
        public int PageMasterId { get; set; }
        public string? PageName { get; set; }
        public string? AliasPageName { get; set; }
        public int? UnderPageMasterId { get; set; }
        public string? PageUrl { get; set; }
        public int? SortId { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
