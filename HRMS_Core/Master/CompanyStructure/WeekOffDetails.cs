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


        public int BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        [ValidateNever]
        public Branch? Branch { get; set; }  



        public string? SundayWeekOffDay { get; set; }    
        public string? MondayWeekOffDay { get; set; }    
        public string? TuesdayWeekOffDay { get; set; }    
        public string? WednesdayWeekOffDay { get; set; }    
        public string? ThursdayWeekOffDay { get; set; }    
        public string? FridayWeekOffDay { get; set; }    
        public string? SaturdayWeekOffDay { get; set; }    

    }
}
