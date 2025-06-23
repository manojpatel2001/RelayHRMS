using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.PrivilegeSetting
{
    public class vmPrevilegeMaster:BaseModel
    {
        public PrivilegeMaster PrivilegeMaster { get; set; }
        public List<PrivilegeDetails> PrivilegeDetails {  get; set; }
    }
}
