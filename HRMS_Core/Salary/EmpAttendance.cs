using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Salary
{
    [Table("EmpAttendance")]
    public class EmpAttendance:BaseModel
    {
        [Key]
        public int? EmpAttendanceId { get; set; }

        public int? Emp_ID { get; set; }

   
        public int? Cmp_ID { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public string? Att_Detail { get; set; }

        public decimal? PresentDays { get; set; }

        public decimal? WeeklyOff { get; set; }

        public int? Holiday { get; set; }

        public decimal? Absent { get; set; }

        public DateTime? System_Date { get; set; }

        public int? Login_Id { get; set; }
    }
}
