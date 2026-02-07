using HRMS_Core.VM;
using HRMS_Core.VM.ExitApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ExitApplication
{
    public interface INOCRepository :IRepository<NOSForm>
    {
        Task<SP_Response> CreateNOC(NOSForm model);
        Task<SP_Response> UpdateNOC(NOSForm model);
        Task<List<NOSForm>> GetNOCByExitApplicationId(int exitApplicationId);
        Task<List<NOCFormDataResponse>> GetNOCByGetEmployeeExitDetails(int EmployeeId);
    }
}
