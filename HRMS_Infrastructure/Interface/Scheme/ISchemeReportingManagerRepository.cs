using HRMS_Core.Master.Scheme;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Scheme
{
    public interface ISchemeReportingManagerRepository
    {
        Task<APIResponse> ManageSchemeReportingManagers(List<SchemeReportingManagerModel> models);
        Task<APIResponse> GetSchemeDropdownDetails();
        Task<APIResponse> GetDrpSchemeDetailsBySchemeType(int schemeTypeId);
        Task<APIResponse> GetReportingByCompanyId(int companyId, int? designationId);
        Task<APIResponse> GetAllEmployByDepartmentId(int? companyId, int? departmentId);
    }
}
