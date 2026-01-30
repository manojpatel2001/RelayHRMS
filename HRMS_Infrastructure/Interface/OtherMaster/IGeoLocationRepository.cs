using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface IGeoLocationRepository
    {
        Task<List<GeoLocation>> GetAllGeoLocations(int companyId);
        Task<SP_Response> CreateGeoLocation(GeoLocation geoLocation);
        Task<SP_Response> UpdateGeoLocation(GeoLocation geoLocation);
        Task<SP_Response> DeleteGeoLocation(DeleteRecordVM deleteRecord);
        Task<SP_Response> CreateAssignGeoLocation(AssignGeoLocation geoLocation);
        Task<SP_Response> DeleteAssignGeoLocation(AssignGeoLocation deleteRecord);
        Task<List<GetAllAssignGeoLocation>> GetAssignGeoLocationsWithLocation(int companyId);
        Task<(List<vmEmployeeListDto> Employees, List<GeoLocation> Locations)> GetAllEmployeeAndLocation(CommonParameter commonParameter);
    }
}
