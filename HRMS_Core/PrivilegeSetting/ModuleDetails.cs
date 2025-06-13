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

namespace HRMS_Core.PrivilegeSetting
{
    [Table("ModuleDetails")]
    public class ModuleDetails:BaseModel
    {
        [Key]
        public int ModuleDetailsId { get; set; }
        public string? ModuleName { get; set; }
        public string? Description { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public CompanyDetails? CompanyDetails { get; set; }
    }
}
