﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Authentication
{
    
        public class TokenResponse
        {
            public string? Token { get; set; }
            public string? Email { get; set; }
            public int? Id { get; set; }

             public string? RoleName { set; get; }
             public string? Slug{ set; get; }
             public List<string>? Permissions { set; get; }
        }
}
