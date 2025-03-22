using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        Task<bool> UpdateDesignation(Designation designation);
        Task<Designation> SoftDelete(DeleteRecordVM DeleteRecord);
    }
}
