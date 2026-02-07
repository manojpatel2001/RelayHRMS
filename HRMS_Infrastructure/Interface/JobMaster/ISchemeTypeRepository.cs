using HRMS_Core.Master.OtherMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface ISchemeTypeRepository : IRepository<SchemeTypeModel>
    {
        Task<List<SchemeTypeModel>> GetAllSchemeType();

    }
}
