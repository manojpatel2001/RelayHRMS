using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CommanReason
{
    public interface ICommonReasonsRepository:IRepository<LeaveCancellationReasonvm>
    {

        Task<List<LeaveCancellationReasonvm>> GetLeavecancellationReasons();

    }
}
