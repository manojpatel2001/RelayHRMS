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
    [Table("Country")]
    public class Country:BaseModel
    {
        [Key]
        public int CountryId { set; get; }
        public string? CountryName { set; get; }
    }
}
