using System;
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
            public string? Id { get; set; }
        }
}
