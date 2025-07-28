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
    public class vmGetAllHolidayMaster
    {
        public int? HolidayMasterId { get; set; }
        public string? HolidayName { get; set; }
        public string? StateName { get; set; }
        public int? StateId { get; set; }
       
        public bool? MultipleHoliday { get; set; } 
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? MessageText { get; set; }
        public string? Holidaycategory { get; set; }

        public bool? RepeatAnnually { get; set; } 
        public bool? IsActive { get; set; } 
       
    }
}
