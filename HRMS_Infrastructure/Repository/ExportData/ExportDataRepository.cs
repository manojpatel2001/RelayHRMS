using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.ExportData;
using HRMS_Infrastructure.Interface.ExportData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.ExportData
{
    public class ExportDataRepository: IExportDataRepository
    {
        private readonly HRMSDbContext _db;
        public ExportDataRepository(HRMSDbContext db)
        {
            _db = db;
        }
        public async Task<List<vmGetAllEmployeeExportData>> GetAllEmployeeExportData(ExportParameter vm)
        {
            try
            {
                var result = await _db.Set<vmGetAllEmployeeExportData>()
                                      .FromSqlInterpolated($"EXEC GetAllEmployeeExportData @CompanyId={vm.CompanyId},@IsLeft={vm.IsLeft}")
                                      .ToListAsync();

                return result ?? new List<vmGetAllEmployeeExportData>();
            }
            catch (Exception ex)
            {
                return new List<vmGetAllEmployeeExportData>();
            }
        }
    }
}
