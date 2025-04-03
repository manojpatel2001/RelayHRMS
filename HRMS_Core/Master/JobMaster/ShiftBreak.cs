using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_Core.DbContext;

namespace HRMS_Core.Master.JobMaster
{
    [Table("ShiftBreak")]
    public class ShiftBreak : BaseModel
    {
        [Key]
        public int BreakID { get; set; }

        public int ShiftID { get; set; }

        [StringLength(50)]
        public required string BreakName { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public bool DeductBreak { get; set; } = false;

        public bool DeductHour { get; set; } = false;

        // Navigation property
        [ForeignKey("ShiftID")]
        public virtual ShiftMaster ShiftMaster { get; set; }
    }

}
