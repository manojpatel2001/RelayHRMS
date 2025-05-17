using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyInformation
{
    public interface IDirectorDetailsRepository:IRepository<DirectorDetails>
    {
        Task<List<vmGetAllDirectorDetails>> GetAllDirectorDetails();
        Task<vmGetAllDirectorDetails?> GetByDirectorDetailsId(int directorDetailsId);
        Task<List<vmGetAllDirectorDetails>> GetDirectorDetailsByCompanyId(int CompanyId);
        Task<VMCommonResult> CreateDirectorDetails(DirectorDetails directorDetails);
        Task<VMCommonResult> UpdateDirectorDetails(DirectorDetails directorDetails);
        Task<VMCommonResult> DeleteDirectorDetails(DeleteRecordVM deleteRecordVM);
    }
}
