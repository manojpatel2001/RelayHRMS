using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface ILeftEmployeeRepository:IRepository<LeftEmployee>
    {
        Task<VMCommonResult> CreateLeftEmployee(LeftEmployee model);
        Task<VMCommonResult> UpdateLeftEmployee(LeftEmployee model);
        Task<VMCommonResult> DeleteLeftEmployee(DeleteRecordVM deleteRecord);
    }
}
