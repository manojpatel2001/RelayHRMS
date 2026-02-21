using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
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
        Task<APIResponse> GetAllManpowerRequisitions(CommonParameter commonParameter);
        Task<APIResponse> GetAllManpowerRequisitionsAdmin(CommonParameter commonParameter);
        Task<APIResponse> GetAllManpowerRequisitionsEss(SearchVmCompOff model);
        Task<APIResponse> CreateManpowerRequisition(ManpowerRequisition manpowerRequisition);
        Task<APIResponse> UpdateManpowerRequisition(ManpowerRequisition manpowerRequisition);
        Task<APIResponse> DeleteManpowerRequisition(DeleteRecordVM model);
        Task<APIResponse> GetDropDownForManpower(int CompanyId);
        Task<APIResponse> GetManpowerRequisitionByManpowerRequisitionId(int ManpowerRequisitionId);
        Task<APIResponse> GetManpowerRequisitionEmailDetails(int? ManpowerRequisitionId);
        Task<APIResponse> GetAllSerialNo(CommonParameter commonParameter);
        Task<APIResponse> UpdateJoinningDetails(UpdateJoinningDetailsModel model);
        Task<APIResponse> GetAllJoiningManpowerRequisitions(CommonParameter commonParameter);
        Task<SP_Response> ApprovalManPower(ManPowerfilter model);
    }

}
