using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    [Table("TicketType")]
    public class TicketType : BaseModel
    {
        [Key]
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }
        public int? DepartmentId { get; set; }

        // Foreign key relationship with the Department class
        [ForeignKey("DepartmentId")]
        public Department? DepartmentDetails { get; set; }
    }

}
