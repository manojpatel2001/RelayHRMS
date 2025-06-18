using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    [Table("EmployeeType")]
    public class EmployeeType:BaseModel
    {
        [Key]
        public int EmployeeTypeId { get; set; }
        public string? EmployeeTypeName { get; set; }
    }
}
