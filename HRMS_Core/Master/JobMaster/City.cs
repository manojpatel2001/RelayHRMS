using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("City")]
    public class City : BaseModel
    {
        [Key]
        public int CityID { get; set; }     
        public string? CityName { get; set; }
        public int? StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        [ValidateNever]
        public State? State { get; set; }   
        public string? Country { get; set; }   
         
        public int? CityCategoryId { get; set; }
        [ForeignKey(nameof(CityCategoryId))]
        [ValidateNever]
        public CityCategory? CityCategory { get; set; } 
        public string? Remarks { get; set; }   
    }
}
