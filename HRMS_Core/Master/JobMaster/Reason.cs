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
    [Table("Reason")]
    public class Reason : BaseModel
    {
        [Key]
        public int ReasonId { get; set; }
        public string? ReasonName { get; set; }
        public string? ReasonType { get; set; }
        public bool IsCommentMandatory { get; set; } = false;
    }
}
