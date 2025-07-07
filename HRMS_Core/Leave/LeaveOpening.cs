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
    [Table("LeaveOpening")]
    public class LeaveOpening:BaseModel
    {
        [Key]
        public int? LeaveOpeningId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? Grade { get; set; }
        public int? LeaveId { get; set; }
        public int? EMP_Id { get; set; }
        public int? comp_id { get; set; }
        public decimal? Opening { get; set; }
    }
}
