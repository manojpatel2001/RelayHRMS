using HRMS_Core.ControlPanel.CompanyInformation;
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

namespace HRMS_Core.PrivilegeSetting
{
    [Table("PrivilegeMaster")]
    public class PrivilegeMaster:BaseModel
    {
        [Key]
        public int PrivilegeMasterId {  get; set; }
        public string? PrivilegeName {  get; set; }
        public int? CompanyId {  get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public CompanyDetails? CompanyDetails {  get; set; }
        public string? BranchId_Multi {  get; set; }
        public string? DepartmentId_Multi {  get; set; }
        public string? VerticalId_Multi {  get; set; }
        public string? PrivilegeType {  get; set; }
    }

}
