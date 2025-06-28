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
    [Table("Earning")]
    public class Earning:BaseModel
    {
        [Key]
        public int EarningId { get; set; }
        public int? EmployeeId { get; set; }
        
        public decimal? Basic { get; set; }
        public decimal? HRA { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Deputation { get; set; }
        public int? Month { get; set; } 
        public int? Year { get; set; }
    }
}
