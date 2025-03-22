using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.CompanyStructure
{
    [Table("HolidayMaster")]
    public class HolidayMaster : BaseModel
    {
        public int HolidayMasterId { get; set; }
        public string? HolidayName { get; set; }
        public string? State { get; set; }
        public int BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        [ValidateNever]
        public Branch? Branch { get; set; }

        public bool MultipleHoliday { get; set; } = false;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string? MessageText { get; set; }
        public string? Holidaycategory { get; set; }

        public bool RepeatAnnually { get; set; } = false;
        public bool HalfDay { get; set; } = false;
        public bool PresentCompulsory { get; set; } = false;
        public bool SMS { get; set; } = false;
        public bool OptionalHoliday { get; set; } = false;  

    }
}
