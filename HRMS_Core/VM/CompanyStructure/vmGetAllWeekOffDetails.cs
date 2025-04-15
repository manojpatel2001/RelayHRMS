using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyStructure
{
    public class vmGetAllWeekOffDetails
    {
        public int? WeekOffDetailsId { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? SundayWeekOffDay { get; set; }
        public string? MondayWeekOffDay { get; set; }
        public string? TuesdayWeekOffDay { get; set; }
        public string? WednesdayWeekOffDay { get; set; }
        public string? ThursdayWeekOffDay { get; set; }
        public string? FridayWeekOffDay { get; set; }
        public string? SaturdayWeekOffDay { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
