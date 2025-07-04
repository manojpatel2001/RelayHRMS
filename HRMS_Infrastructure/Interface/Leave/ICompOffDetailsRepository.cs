using HRMS_Core.EmployeeMaster;
using HRMS_Core.Leave;
using HRMS_Core.VM;
using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ICompOffDetailsRepository:IRepository<Comp_Off_Details>
    {
        Task<bool> InsertCompOffAsync(Comp_Off_Details model);
        Task<bool> Updateapproval(int Emp,string Status);
    }
}
