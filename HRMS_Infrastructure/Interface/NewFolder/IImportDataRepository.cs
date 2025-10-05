using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.NewFolder
{
    public interface IImportDataRepository
    {
        Task<ImportSPResult> ImportAttendance(string jsonData, string createdBy);
        Task<ImportSPResult> ImportMonthlyEarnings(string jsonData, string createdBy);
        Task<ImportSPResult> ImportMonthlyDeductions(string jsonData, string createdBy);
        Task<ImportSPResult> ImportLeaveOpening(string jsonData, string createdBy);
    }
    public class ImportSPResult
    {
        public int? InsertedCount { get; set; }
        public int? ErrorCount { get; set; }
        public int? DuplicateCount { get; set; }
        public int? BlankCount { get; set; }
        public List<ImportError> Errors { get; set; } = new List<ImportError>();
    }

    
    public class ImportError
    {
        public int? RowNumber { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }


}
