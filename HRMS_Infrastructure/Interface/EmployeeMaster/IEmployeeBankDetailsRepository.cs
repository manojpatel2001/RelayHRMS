using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IEmployeeBankDetailsRepository
    {
        Task<APIResponse> CreateEmployeeBankDetails(EmployeeBankDetailsModel model);
        Task<APIResponse> UpdateEmployeeBankDetails(EmployeeBankDetailsModel model);
        Task<APIResponse> DeleteEmployeeBankDetails(DeleteRecordVM delete);
        Task<APIResponse> GetAllEmployeeBankDetails();
    }
}
