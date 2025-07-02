using HRMS_Core.SuperAdmin;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.SuperAdmin
{
    public interface ISuperAdminDetailsRepository:IRepository<SuperAdminDetails>
    {
        Task<SuperAdminDetails?> GetSuperAdminByCredentials(vmLogin vmLogin);
    }
}
