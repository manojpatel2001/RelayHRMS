﻿using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    [Table("Category")]
    public class Category:BaseModel
    {
        [Key]
        public int CategoryId {  get; set; }
        public string? CategoryName {  get; set; }
    }
}
