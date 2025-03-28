using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("Grade")]
    public class Grade : BaseModel
    {
        [Key]
        public int GradeId { get; set; }
        public string? GradeName { get; set; }
        public string? Description { get; set; }
        public int? BasicSalary { get; set; }
        public int? Govt_MinimumBasics { get; set; }
        public int? Sorting_No	{ get; set; }
        public int? NoticePeriodDays { get; set; }
        public int? NoticePeriodWorkingDays { get; set; }
        public string? SalaryRange	{ get; set; }
        public int? EligibilityAmount{ get; set; }
        public string? WagesType { get; set; }
        public bool OverTimeApplicable { get; set; } = false;   
    }
}
