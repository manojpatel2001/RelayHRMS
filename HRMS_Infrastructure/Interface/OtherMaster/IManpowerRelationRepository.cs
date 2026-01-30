using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface IManpowerRelationRepository
    {
        Task<APIResponse> CreateManpowerRelation(ManpowerRelationModel model);
        Task<APIResponse> UpdateManpowerRelation(ManpowerRelationModel model);
        Task<APIResponse> DeleteManpowerRelation(DeleteRecordVM delete);
        Task<APIResponse> GetAllManpowerRelation(int manpowerRequisitionId);
    }

}
