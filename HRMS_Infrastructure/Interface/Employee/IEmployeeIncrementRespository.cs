using HRMS_Core.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeIncrementRespository
    {
        Task<List<IncrementReason>> GetAllIncrementReason();
    }
}
