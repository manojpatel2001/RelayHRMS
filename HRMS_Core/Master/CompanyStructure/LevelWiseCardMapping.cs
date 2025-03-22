using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.CompanyStructure
{
    [Table("LevelWiseCardMapping")]
    public class LevelWiseCardMapping : BaseModel
    {
        [Key]
        public int LevelWiseCardMappingId { get; set; } 

        public string? Level { get; set; }   
        public int CardNo { get; set; }
        public string? CardType { get; set; }    
    }
}
