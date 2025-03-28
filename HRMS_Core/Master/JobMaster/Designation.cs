using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("Designation")]
    public class Designation : BaseModel
    {
        [Key]
        public int DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public string? DesignationCode { get; set; }
        public int? SortingNo { get; set; }
        public bool ManagerialPost { get; set; } = false;
        public bool IsMain { get; set; } = false;
        public string? Allow_ReimEligibilityAmount { get; set; }
        public bool? AbscondingReminderEmail { get; set; } = false;

    }
}
