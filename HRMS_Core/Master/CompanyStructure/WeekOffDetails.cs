using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.CompanyStructure
{
    [Table("WeekOffDetails")]
    public class WeekOffDetails : BaseModel
    {
        [Key]
        public int WeekOffDetailsId { get; set; }   

        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string? WeekOffName { get; set; }    
        public string? WeekOffDay { get; set; }    
       

    }
}
