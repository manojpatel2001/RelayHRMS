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
    public interface IManpowerRequisitionRepository
    {
        Task<List<ManpowerRequisitionViewModel>> GetAllManpowerRequisitions(CommonParameter commonParameter);
        Task<SP_Response> CreateManpowerRequisition(ManpowerRequisition manpowerRequisition);
        Task<SP_Response> UpdateManpowerRequisition(ManpowerRequisition manpowerRequisition);
        Task<SP_Response> DeleteManpowerRequisition(DeleteRecordVM model);
        Task<APIResponse> GetDropDownForManpower(int CompanyId);
        Task<APIResponse> GetManpowerRequisitionByManpowerRequisitionId(int ManpowerRequisitionId);
        Task<APIResponse> GetAllSerialNo(CommonParameter commonParameter);
        Task<APIResponse> UpdateJoinningDetails(UpdateJoinningDetailsModel model);
    }

}
