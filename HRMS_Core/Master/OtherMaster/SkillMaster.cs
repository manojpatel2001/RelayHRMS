using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    public class SkillMaster
    {      
             public int SkillMasterId { get; set; }
            public string? SkillName { get; set; }
            public bool? IsDeleted { get; set; }
            public bool? IsEnabled { get; set; }
            public string? CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string?UpdatedBy { get; set; }
            public DateTime? UpdatedDate{ get; set; }
            public string?DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }
        
    }
}
