using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.OtherMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeHolidayMarkingRepository:IRepository<EmployeeHolidayMarking>
    {
        Task<SP_Response> CreateEmployeeHoliday(EmployeeHolidayMarking model);
        Task<SP_Response> UpdateEmployeeHoliday(EmployeeHolidayMarking model);
        Task<SP_Response> DeleteEmployeeHoliday(int id , int DeletedBy);
        Task<EmployeeHolidayMarking?> GetEmpHolidayById(vmCommonGetById filter); 
        Task<List<EmployeeHolidayMarkingViewModel>> GetAllEmployeeHoliday(); 
      
    }
}
