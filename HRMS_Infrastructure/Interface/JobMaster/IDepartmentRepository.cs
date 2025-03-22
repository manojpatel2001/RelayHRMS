using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<bool> UpdateDepartment(Department department);
        Task<Department> SoftDelete(DeleteRecordVM DeleteRecord);
    }
}
