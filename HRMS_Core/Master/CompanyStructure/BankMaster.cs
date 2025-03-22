using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.CompanyStructure
{
    [Table("BankMaster")]
    public class BankMaster : BaseModel
    {
        [Key]
        public int BankMasterId { get; set; }   
        public string? BankName { get; set; }  
        public string? BankCode{ get; set; }  
        public string? BranchName{ get; set; }  
        public string? AccountNo{ get; set; }  
        public string? Address { get; set; }  
        public string? City { get; set; }  
        public string? BankBSRCode { get; set; }  
        public bool IsDefaultBank { get; set; }  
    }
}
