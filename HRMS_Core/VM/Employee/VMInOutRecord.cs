﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class VMInOutRecord
    {
        public DateTime For_Date { get; set; }
        public string Day_Name { get; set; }
        public string In_Time { get; set; }
        public string Out_Time { get; set; }
        public string Duration { get; set; }
    }
}
