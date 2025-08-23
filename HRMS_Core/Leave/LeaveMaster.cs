using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Leave
{
    [Table("LeaveMaster")] 
    public class LeaveMaster : BaseModel
    {
        [Key]
        public int? Leave_TypeId { get; set; }
        public int? Comp_Id { get; set; }
        public string? LeaveType { get; set; }
        public string? Leave_Name { get; set; }

    }
}
