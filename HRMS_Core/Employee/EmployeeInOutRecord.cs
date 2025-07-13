﻿using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    [Table("EmployeeInOutRecord")]
    public class EmployeeInOutRecord:BaseModel
    {
        [Key]
        public int Emp_IO_Id { get; set; }     
        public int? Emp_Id { get; set; }
       
        public int? Com_Id { get; set; }
        
        public DateTime? For_Date { get; set; }
        public DateTime? In_Time { get; set; }
        public DateTime? Out_Time { get; set; }
        public string? Duration { get; set; }
        public string? Reason { get; set; }
        public string? Ip_adrress { get; set; }
        public DateTime? In_Date_Time { get; set; }
        public DateTime? Out_Date_Time { get; set; }

        public string? Skip_Count { get; set; }
        public string? Late_Calc_Not_App { get; set; }
        public string? Chk_By_Superior { get; set; }
        public string? Sup_Comment { get; set; }
        public string? Half_Full_day { get; set; }
        public string? Is_Cancel_Late_In { get; set; }
        public string? Is_Cancel_Early_Out { get; set; }
        public string? Is_Default_In { get; set; }
        public string? Is_Default_Out { get; set; }
        public string? Cmp_prp_in_flag { get; set; }
        public string? Cmp_prp_out_flag { get; set; }
        public string? is_Cmp_purpose { get; set; }
        public string? App_Date { get; set; }
        public string? Apr_Date { get; set; }
        public DateTime? System_date { get; set; }
        public string? Other_Reason { get; set; }
        public string? ManualEntryFlag { get; set; }
        public string? statusFlag { get; set; }
        public DateTime? In_Admin_Time { get; set; }
        public DateTime? Out_Admin_Time { get; set; }
        public string? PunchType{ get; set; }

    }
}
