using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    [Table("PasswordHistory")]
    public class PasswordHistory : BaseModel
    {
        public int PasswordHistoryId { get; set; }
        public int EMPID { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
