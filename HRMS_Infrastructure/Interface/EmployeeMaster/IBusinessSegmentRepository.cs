using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IBusinessSegmentRepository
    {
        Task<VMCommonResult> CreateBusinessDetail(BusinessSegment model);
        Task<VMCommonResult> UpdateBusinessDetail(BusinessSegment model);
        Task<VMCommonResult> DeleteBusinessDetail(DeleteRecordVM deleteRecord);
        Task<BusinessSegment?> GetBusinessDetailById(vmCommonGetById vmCommonGetById);
        Task<List<BusinessSegment>> GetAllBusinessSegments(vmCommonGetById vmCommonGetById);
    }
}
