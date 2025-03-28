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
    [Table("Allowance_Deduction")]
    public class Allowance_Deduction : BaseModel
    {
        [Key]
        public int Allowance_DeductionId { get; set; }
        public string? ShortName { get; set; }
        public string? AllowanceType { get; set; }
        public string? CalculateOn { get; set; }
        public string? Type { get; set; }
        public int SortingNumber { get; set; }
        public string? DefID { get; set; } 
        public bool Optional { get; set; } = false;
        public string? Code { get; set; }

    }
}
