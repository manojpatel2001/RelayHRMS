using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    [Table("TicketPriority")]
    public class TicketPriority:BaseModel
    {
        [Key]
        public int? TicketPriorityId { get; set; }
        public string? TicketPriorityName { get; set; }
        public int? EscalationHours { get; set; }
    }
}
