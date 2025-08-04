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
    [Table("LeaveDetails")]
    public class LeaveDetails : BaseModel
    {
        [Key]
        public int LeaveDetailsId { get; set; }
        public int? Emp_Id { get; set; }
        public int? Comp_Id { get; set; }
         public string? Comp_Off_Leave { get; set; }
        public string? Half_Leave { get; set; }
        public string? LWP { get; set; }
        public string? Privilege_Leave { get; set; }







    }
}
