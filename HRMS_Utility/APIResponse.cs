using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Utility
{
    public class APIResponse
    {
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
        public bool isSuccess { get; set; }
    }
}
