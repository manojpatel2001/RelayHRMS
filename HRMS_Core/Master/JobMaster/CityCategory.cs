using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    public class CityCategory : BaseModel
    {
        [Key]
        public int CityCategoryId { get; set; } 
        public string? CityCategoryName { get; set; }  
        public string? Description { get; set; }  
    }
}
