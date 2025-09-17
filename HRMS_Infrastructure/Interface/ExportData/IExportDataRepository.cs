using HRMS_Core.VM.ExportData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ExportData
{
    public interface IExportDataRepository
    {
        Task<List<vmGetAllEmployeeExportData>> GetAllEmployeeExportData(int CompanyId);
    }
}
