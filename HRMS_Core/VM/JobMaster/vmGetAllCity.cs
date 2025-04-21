using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.JobMaster
{
  
       
        public class vmGetAllCity
        {
            public int CityID { get; set; }
            public string? CityName { get; set; }
            public int? StateId { get; set; }
            public string? StateName { get; set; }
            public string? Country { get; set; }
            public int? CityCategoryId { get; set; }
            public string? CityCategoryName { get; set; } 
            public string? Remarks { get; set; }
            public bool? IsDeleted { get; set; }
            public bool? IsEnabled { get; set; }
            public DateTime CreatedDate { get; set; }
            public string? CreatedBy { get; set; }
        }
    
 
    
}
