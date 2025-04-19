using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyStructure
{
    public interface ILevelWiseCardMappingRepository : IRepository<LevelWiseCardMapping>
    {
        Task<List<LevelWiseCardMapping>> GetAllLevelWiseCardMapping();
        Task<LevelWiseCardMapping?> GetByLevelWiseCardMappingId(int levelWiseCardMappingId);
        Task<VMCommonResult> CreateLevelWiseCardMapping(LevelWiseCardMapping levelWiseCardMapping);
        Task<VMCommonResult> UpdateLevelWiseCardMapping(LevelWiseCardMapping levelWiseCardMapping);
        Task<VMCommonResult> DeleteLevelWiseCardMapping(DeleteRecordVM deleteRecordVM);
    }
}
