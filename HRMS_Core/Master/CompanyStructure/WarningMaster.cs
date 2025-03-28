using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.CompanyStructure
{
    [Table("WarningMaster")]
    public class WarningMaster : BaseModel
    {
        [Key]
        public int WarningMasterId { get; set; }    

        public string? WarningName { get; set; } 
        public string? Level { get; set; }
        public string? Remarks { get; set; }
        public string? DeductionType { get; set; }
    }
}
