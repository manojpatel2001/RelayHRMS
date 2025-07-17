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
    public interface IRelationShipRepository
    {
        Task<VMCommonResult> CreateRelationShip(RelationShip model);
        Task<VMCommonResult> UpdateRelationShip(RelationShip model);
        Task<VMCommonResult> DeleteRelationShip(DeleteRecordVM deleteRecord);
        Task<EmergencyContact?> GetRelationShipById(vmCommonGetById vmCommonGetById);
        Task<List<RelationShip>> GetAllRelationShips(vmCommonGetById vmCommonGetById);
    }
}
