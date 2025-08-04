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
        public int? CompanyId { get; set; }
        public string? WeekOffName { get; set; }
        public string? WeekOffDay { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
