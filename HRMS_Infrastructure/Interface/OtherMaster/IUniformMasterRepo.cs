using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface IUniformMasterRepo:IRepository<UniformMaster>  
    {
        Task<SP_Response> CreateUniformMaster(UniformMaster model);
        Task<SP_Response> UpdateUniformMaster(UniformMaster model);
        Task<SP_Response> DeleteUniformMaster(DeleteRecordVM deleteRecord);
        Task<List<UniformMaster>> GetAllUniformMaster();
    }
}
