using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{

    [Table("OrganizationPolicy")]
    public class OrganizationPolicy: BaseModel
    {
        [Key]
        public int OrganizationPolicyId {  get; set; }
        public string? OrganizationPolicyName {  get; set; }
        public string? ToolTip {  get; set; }
        public DateTime? FromDate {  get; set; }
        public DateTime? ToDate  {  get; set; }
        public int? Sorting  {  get; set; }
        public string? DocumentUrl  {  get; set; }
        public string? Grouping  {  get; set; }
        public string? GroupingValue  {  get; set; }

    }
}
