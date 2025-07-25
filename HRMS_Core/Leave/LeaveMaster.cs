﻿using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Leave
{
    [Table("LeaveMaster")] 
    public class LeaveMaster : BaseModel
    {
        [Key]
        public int? Leave_TypeId { get; set; }
        public int? Comp_Id { get; set; }
        public string? Leave_Name { get; set; }
        public string? Leave_Type { get; set; }
        public string? Leave_Paid_Unpaid { get; set; }

        public string? TotalLeave { get; set; }
        public int? Carry_forword_leave { get; set; }

    }
}
