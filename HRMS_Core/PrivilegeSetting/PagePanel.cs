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
    [Table("PagePanel")]
    public class PagePanel:BaseModel
    {
        [Key]
        public int PagePanelId {  get; set; }
        public string? PagePanelName{  get; set; }
        public string? AliasPanel{  get; set; }
    }
}
