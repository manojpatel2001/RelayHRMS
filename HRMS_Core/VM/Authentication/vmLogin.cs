using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM
{
    public  class vmLogin
    {
        public string Email {  get; set; }
        public string Password {  get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
