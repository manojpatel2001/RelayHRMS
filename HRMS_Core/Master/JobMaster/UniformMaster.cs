using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.JobMaster
{
    public class UniformMaster
    {
        public int UniformID { get; set; }
        public string? UniformName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? UniformRate { get; set; }
        public decimal? UniformDeductInstallment { get; set; }
        public decimal? UniformRefundInstallment { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        //public DateTime? DeletedDate { get; set; }
        //public string DeletedBy { get; set; }
    }
}
