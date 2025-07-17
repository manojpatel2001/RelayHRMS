using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmegencyContacts
{
    public interface IEmergencyContactRepository
    {
        Task<VMCommonResult> CreateEmergencyContact(EmergencyContact model);
        Task<VMCommonResult> UpdateEmergencyContact(EmergencyContact model);
        Task<VMCommonResult> DeleteEmergencyContact(DeleteRecordVM deleteRecord);
        Task<EmergencyContact?> GetEmergencyContactById(vmCommonGetById vmCommonGetById);
        Task<List<EmergencyContact>> GetAllEmergencyContacts(vmCommonGetById vmCommonGetById);
    }
}
