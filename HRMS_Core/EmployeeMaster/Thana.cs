using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    [Table("Thana")]
    public class Thana:BaseModel
    {
        [Key]
        public int ThanaId { set; get; }
        public string? ThanaName { set; get; }
    }
}
