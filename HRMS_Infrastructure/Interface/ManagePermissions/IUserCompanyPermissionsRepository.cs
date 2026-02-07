using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ManagePermissions
{
    public  interface IUserCompanyPermissionsRepository
    {
        Task<SP_Response> CreateUserCompanyPermissions(VMUserCompanyPermission model);
        Task<SP_Response> UpdateUserCompanyPermissions(VMUserCompanyPermission model);
        Task<SP_Response> DeleteUserCompanyPermissions(DeleteRecordVM model);
        Task<List<vmGetAllCompanyDetailsList>> GetCompanyPermissionsListByEmployeeId(int EmployeeId);
    }
}
