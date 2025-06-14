using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.PrivilegeSetting
{
    [Table("PrivilegeDetails")]
    public class PrivilegeDetails:BaseModel
    {
        [Key]
        public int PrivilegeDetailsId { get; set; }
        public int? PrivilegeMasterId { get; set; }
        [ForeignKey(nameof(PrivilegeMasterId))]
        [ValidateNever]
        public PrivilegeMaster? PrivilegeMaster { get; set; }
        public int? PageId { get; set; }
        [ForeignKey(nameof(PageId))]
        [ValidateNever]
        public PageMaster? PageMaster { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public CompanyDetails? CompanyDetails { get; set; }
        public bool? Is_View { get; set; } = false;
        public bool? Is_Edit { get; set; } = false;
        public bool? Is_Save { get; set; } = false;
        public bool? Is_Delete { get; set; } = false;
    }
}
