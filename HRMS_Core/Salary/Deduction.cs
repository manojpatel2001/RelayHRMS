using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Salary
{
    [Table("Deduction")]
    public class Deduction:BaseModel
    {
        [Key]
        public int DeductionId { get; set; }
        public int? EmployeeId { get; set; }
        
        public decimal? PF { get; set; }
        public decimal? ESIC { get; set; }
        public decimal? PT { get; set; }
        public decimal? Insurance { get; set; }
        public decimal? LWF { get; set; }
        public decimal? TDS { get; set; }
    }
}
