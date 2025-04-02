using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("ShiftMaster")]
    public class ShiftMaster
    {
        [Key]
        public int ShiftID { get; set; }

        [Required]
        [StringLength(255)]
        public required string ShiftName { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public bool IsHalfDay { get; set; } = false;

        public bool AutoShiftChange { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public bool IsTrainingShift { get; set; } = false;

        public bool IsSplitShift { get; set; } = false;

        // Navigation property
        public virtual ICollection<ShiftBreak> ShiftBreaks { get; set; }
    }

}
