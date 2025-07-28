using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<VMCommonResult> CreateDepartment(Department model);
        Task<VMCommonResult> UpdateDepartment(Department model);
        Task<VMCommonResult> DeleteDepartment(DeleteRecordVM deleteRecord);
        Task<List<Department>> GetAllDepartments(vmCommonGetById filters);
        Task<Department?> GetDepartmentById(vmCommonGetById filter);
        Task<vmCheckExistDepartmentCode?> CheckExistDepartmentCode(vmCommonGetById filter);
    }
}
