using HRMS_Core.DbContext;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    [Table("Department")]
    public class Department : BaseModel
    {
        [Key] 
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? Code { get; set; }
        public int? SortingNo { get; set; }
        public int? MinimumWages { get; set; }
        public bool OJTApplicable { get; set; } = false;


    }
}
