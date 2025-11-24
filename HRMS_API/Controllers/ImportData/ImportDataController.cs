using ExcelDataReader;
using HRMS_Core.ControlPanel.ImportData;
using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.NewFolder;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ImportDataController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;
    private readonly HRMSDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public ImportDataController(IUnitOfWork unitOfWork, IWebHostEnvironment env, HRMSDbContext dbContext, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _env = env;
        _dbContext = dbContext;
        _configuration = configuration;
    }

   
    private class ImportCache
    {
        public Dictionary<string, int> BranchIdByCode { get; set; } = new();
        public Dictionary<string, int> BranchIdByName { get; set; } = new();
        public HashSet<string> ExistingBranches { get; set; } = new();
        public HashSet<string> ExistingDepartments { get; set; } = new();
        public HashSet<string> ExistingDesignations { get; set; } = new();
        public HashSet<string> ExistingBanks { get; set; } = new();
        public Dictionary<string, int> StateIdByName { get; set; } = new();
        public HashSet<string> ExistingCities { get; set; } = new();
        public HashSet<string> ExistingHolidays { get; set; } = new();
    }

    [HttpPost("Upload")]
    public async Task<APIResponse> Upload([FromForm] ImportRequest request)
    {
        try
        {
            if (request.File == null || request.RowFrom <= 0 || request.RowTo < request.RowFrom)
                return new APIResponse { isSuccess = false, ResponseMessage = "Invalid input" };
        
            // Step 1: Parse Excel File
            var dt = await ParseExcelFile(request);
            if (dt == null)
                return new APIResponse { isSuccess = false, ResponseMessage = "Failed to parse Excel file" };

            // Step 2: Validate Template
            if (!ValidateTemplateColumns(dt, request.Type, out string headerError))
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = headerError
                };
            }

            // Step 3: Route to SP or API based on import type
            if (IsLargeVolumeImport(request.Type))
            {
                // Use Stored Procedure for large volume imports
                return await ProcessWithStoredProcedure(dt, request.Type, request.RowFrom ,request.CreatedBy);
            }
            else
            {
                // Use API logic for small master imports
                return await ProcessWithAPI(dt, request.Type, request.RowFrom ,request.CreatedBy);
            }
        }
        catch (Exception ex)
        {
            return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
        }
    }

    private async Task<DataTable> ParseExcelFile(ImportRequest request)
    {
        if (request.File == null || string.IsNullOrEmpty(request.SheetName))
            return null;

        if (request.RowTo < request.RowFrom)
            return null;

        var dt = new DataTable();
        var extension = Path.GetExtension(request.File.FileName).ToLower();
        using var stream = new MemoryStream();
        await request.File.CopyToAsync(stream);
        stream.Position = 0;

        try
        {
            if (extension == ".xlsx")
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[request.SheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    return null;

                int colCount = worksheet.Dimension.End.Column;
                for (int col = 1; col <= colCount; col++)
                    dt.Columns.Add(worksheet.Cells[1, col]?.Text ?? $"Column {col}");

                int startRow = Math.Max(request.RowFrom, 2);
                int endRow = request.RowTo;
                for (int row = startRow; row <= endRow; row++)
                {
                    var dataRow = dt.NewRow();
                    bool isRowEmpty = true;
                    for (int col = 1; col <= colCount; col++)
                    {
                        var cellValue = worksheet.Cells[row, col]?.Text?.Trim() ?? string.Empty;
                        dataRow[col - 1] = cellValue;
                        if (!string.IsNullOrEmpty(cellValue))
                            isRowEmpty = false;
                    }
                    if (!isRowEmpty)
                        dt.Rows.Add(dataRow);
                }
            }
            else if (extension == ".xls")
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using var reader = ExcelReaderFactory.CreateBinaryReader(stream);
                var result = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration { UseHeaderRow = false }
                });
                var sheet = result.Tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == request.SheetName);
                if (sheet == null)
                    return null;

                for (int col = 0; col < sheet.Columns.Count; col++)
                    dt.Columns.Add(sheet.Rows[0][col]?.ToString() ?? $"Column {col}");

                int startRow = Math.Max(request.RowFrom - 1, 1);
                int endRow = request.RowTo - 1;
                for (int row = startRow; row <= endRow && row < sheet.Rows.Count; row++)
                {
                    var sourceRow = sheet.Rows[row];
                    var dataRow = dt.NewRow();
                    bool isRowEmpty = true;
                    for (int col = 0; col < sheet.Columns.Count; col++)
                    {
                        var value = sourceRow[col]?.ToString()?.Trim() ?? string.Empty;
                        dataRow[col] = value;
                        if (!string.IsNullOrEmpty(value))
                            isRowEmpty = false;
                    }
                    if (!isRowEmpty)
                        dt.Rows.Add(dataRow);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing Excel: {ex.Message}");
            return null;
        }

        return dt.Rows.Count > 0 ? dt : null;
    }


    private bool IsLargeVolumeImport(string type)
    {
    
        return type == "Attendance" ||
               type == "MonthlyEar" ||
               type == "MonthlyDed" ||
               type == "LeaveOpening";
    }

    private async Task<APIResponse> ProcessWithStoredProcedure(DataTable dt, string type, int startRow ,string createdBy)
    {
        try
        {
        
            var jsonData = ConvertDataTableToJson(dt, type, startRow);

        
            ImportSPResult result = null;

            // Call appropriate repository method based on import type
            switch (type)
            {
                case "Attendance":
                    result = await _unitOfWork.importDataRepository.ImportAttendance(jsonData, createdBy);
                    break;

                case "MonthlyEar":
                    result = await _unitOfWork.importDataRepository.ImportMonthlyEarnings(jsonData , createdBy);
                    break;

                case "MonthlyDed":
                    result = await _unitOfWork.importDataRepository.ImportMonthlyDeductions(jsonData , createdBy);
                    break;

                case "LeaveOpening":
                    result = await _unitOfWork.importDataRepository.ImportLeaveOpening(jsonData , createdBy);
                    break;

                default:
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = $"Invalid import type: {type}"
                    };
            }

            // Convert errors to API response format
            var errors = result.Errors.Select(e => new
            {
                rowNumber = e.RowNumber,
                employeeCode = e.EmployeeCode,
                importDate = DateTime.Now.ToString("dd-MM-yyyy"),
                errorDescription = e.ErrorMessage,
                actualValue = e.EmployeeCode,
                suggestion = e.ErrorType,
                importType = type
            }).ToList();

            // Create success message
            string msg = $"{result.InsertedCount} row(s) inserted successfully.";
            if (result.DuplicateCount > 0 || result.BlankCount > 0)
            {
                msg += $" {result.DuplicateCount} duplicate(s) and {result.BlankCount} blank row(s) were skipped.";
            }

            return new APIResponse
            {
                isSuccess = result.InsertedCount > 0,
                ResponseMessage = msg,
                Data = errors.Any() ? errors : null
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                isSuccess = false,
                ResponseMessage = $"Import failed: {ex.Message}"
            };
        }
    }
    private string ConvertDataTableToJson(DataTable dt, string type, int startRow)
    {
        var rows = new List<Dictionary<string, object>>();
        int currentRow = startRow;

        foreach (DataRow row in dt.Rows)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dict[dt.Columns[i].ColumnName] = row[i]?.ToString() ?? "";
            }

            dict["RowNumber"] = currentRow++;
            rows.Add(dict);
        }

        return JsonSerializer.Serialize(rows);
    }

 
    private async Task<APIResponse> ProcessWithAPI(DataTable dt, string type, int startRow ,string createdBy)
    {
        var cache = await LoadCacheData(type);

        int insertedCount = 0, duplicateCount = 0, blankCount = 0;
        var errorRows = new List<object>();
        int batchCounter = 0;
        const int batchSize = 100;

        foreach (DataRow row in dt.Rows)
        {
            int rowIndex = dt.Rows.IndexOf(row) + startRow;
            var result = await ProcessRowByType(row, type, rowIndex, cache, createdBy);

            if (result.IsSuccess)
                insertedCount++;
            else
            {
                if (result.IsDuplicate) duplicateCount++;
                if (result.IsBlank) blankCount++;
                errorRows.Add(result.ErrorRow);
            }

            batchCounter++;
            if (batchCounter >= batchSize)
            {
                await _unitOfWork.CommitAsync();
                batchCounter = 0;
            }
        }

        if (batchCounter > 0)
            await _unitOfWork.CommitAsync();

        string msg = $"{insertedCount} row(s) inserted.";
        if (duplicateCount > 0 || blankCount > 0)
            msg += $" {duplicateCount} duplicate(s), {blankCount} blank(s) skipped.";

        return new APIResponse
        {
            isSuccess = insertedCount > 0,
            ResponseMessage = msg,
            Data = errorRows.Any() ? errorRows : null
        };
    }

    private async Task<ImportCache> LoadCacheData(string type)
    {
        var cache = new ImportCache();

        switch (type)
        {
            case "Branch":
                var branches = await _dbContext.Branch
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(b => new { b.BranchCode, b.BranchName, b.BranchId })
                    .ToListAsync();

                foreach (var b in branches)
                {
                    cache.ExistingBranches.Add($"{b.BranchCode?.ToLower()}_{b.BranchName?.ToLower()}");
                    cache.BranchIdByCode[b.BranchCode?.ToLower() ?? ""] = b.BranchId;
                    cache.BranchIdByName[b.BranchName?.ToLower() ?? ""] = b.BranchId;
                }
                break;

            case "Department":
                var depts = await _dbContext.Departments
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(d => d.DepartmentName)
                    .ToListAsync();
                cache.ExistingDepartments = depts.Select(d => d?.ToLower() ?? "").ToHashSet();
                break;

            case "Designation":
                var desigs = await _dbContext.Designations
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(d => d.DesignationName)
                    .ToListAsync();
                cache.ExistingDesignations = desigs.Select(d => d?.ToLower() ?? "").ToHashSet();
                break;

            case "Bank":
                var banks = await _dbContext.BankMaster
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(b => b.BankCode)
                    .ToListAsync();
                cache.ExistingBanks = banks.Select(b => b?.ToLower() ?? "").ToHashSet();
                break;

            case "City":
                var states = await _dbContext.States
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(s => new { s.StateName, s.StateId })
                    .ToListAsync();
                cache.StateIdByName = states.ToDictionary(s => s.StateName?.ToLower() ?? "", s => s.StateId);

                var cities = await _dbContext.City
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(c => new { c.CityName, c.StateId })
                    .ToListAsync();
                cache.ExistingCities = cities.Select(c => $"{c.CityName?.ToLower()}_{c.StateId}").ToHashSet();
                break;

            case "Holiday":
                var branches2 = await _dbContext.Branch
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(b => new { b.BranchName, b.BranchId })
                    .ToListAsync();
                cache.BranchIdByName = branches2.ToDictionary(b => b.BranchName?.ToLower() ?? "", b => b.BranchId);

                var holidays = await _dbContext.HolidayMaster
                    .Where(x => x.IsEnabled == true && x.IsDeleted != true)
                    .Select(h => h.HolidayName)
                    .ToListAsync();
                cache.ExistingHolidays = holidays.Select(h => h?.ToLower() ?? "").ToHashSet();
                break;
        }

        return cache;
    }

    private bool ValidateTemplateColumns(DataTable dt, string type, out string error)
    {
        error = "";
        List<string> expectedHeaders = new();

        switch (type)
        {
            case "Branch":
                expectedHeaders = new List<string> { "Branch_Code", "Branch_Name", "Country_Name" };
                break;
            case "Department":
                expectedHeaders = new List<string> { "Department_Name" };
                break;
            case "Designation":
                expectedHeaders = new List<string> { "Designation_Name", "Designation_Code" };
                break;
            case "Bank":
                expectedHeaders = new List<string> { "Bank_Name", "Bank_Code", "Branch_Name", "Account_No", "Address", "City", "Bank_BSR_Code", "IS_Default" };
                break;
            case "City":
                expectedHeaders = new List<string> { "City_Name", "State_Name", "Country", "Category", "Entry_Type" };
                break;
            case "Holiday":
                expectedHeaders = new List<string> { "Holiday_Name", "Branch_Name", "From_Date", "To_Date", "Holiday_Category", "Repeat_Annually", "Half_Day", "Present_Compulsory", "Optional_Holiday", "Approval_Max_Limit", "Unpaid_Holiday" };
                break;
            case "MonthlyEar":
                expectedHeaders = new List<string> { "Alpha_Emp_code", "Month", "Year", "Basic", "HRA", "Conveyance", "Medical", "Deputation", "ChildEducationAllowance", "Arrears" };
                break;
            case "MonthlyDed":
                expectedHeaders = new List<string> { "Alpha_Emp_code", "Month", "Year", "PF", "ESIC", "PT", "LWF", "TDS", "TermInsurance", "GroupMedical", "Loan", "OtherDeduction" };
                break;
            case "Attendance":
                expectedHeaders = new List<string> { "Alpha Emp Code", "Month", "Year", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                break;
            case "LeaveOpening":
                expectedHeaders = new List<string> { "Alpha_Emp_Code", "LeaveName", "Leave_Credit_Days", "For_Date" };
                break;
            default:
                error = $"No template defined for type '{type}'";
                return false;
        }

        for (int i = 0; i < expectedHeaders.Count; i++)
        {
            if (dt.Columns.Count <= i || dt.Columns[i].ColumnName.Trim() != expectedHeaders[i].Trim())
            {
                error = $"Template mismatch: Column {i + 1} must be '{expectedHeaders[i]}', but found '{(dt.Columns.Count > i ? dt.Columns[i].ColumnName : "missing")}' .";
                return false;
            }
        }

        return true;
    }

    private async Task<(bool IsSuccess, bool IsDuplicate, bool IsBlank, object ErrorRow)> ProcessRowByType(
        DataRow row, string type, int rowIndex, ImportCache cache ,string createdBy)
    {
        if (type == "Branch")
        {
            string code = row[0]?.ToString()?.Trim();
            string name = row[1]?.ToString()?.Trim();
            string country = row[2]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(country))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            string key = $"{code?.ToLower()}_{name?.ToLower()}";
            if (cache.ExistingBranches.Contains(key))
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Branch", code, "Branch with same code and name already exists.", type));

            await _unitOfWork.BranchRepository.AddAsync(new Branch
            {
                BranchCode = code,
                BranchName = name,
                CountryName = country,
                IsEnabled = true,
                IsDeleted = false,
                 CreatedBy = createdBy,
            });

            cache.ExistingBranches.Add(key);
            return (true, false, false, null);
        }
        else if (type == "Department")
        {
            string name = row[0]?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            if (cache.ExistingDepartments.Contains(name?.ToLower() ?? ""))
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Department", name, "Enter unique Department name.", type));

            await _unitOfWork.DepartmentRepository.AddAsync(new Department
            {
                DepartmentName = name,
                IsEnabled = true,
                IsDeleted = false,
                CreatedBy = createdBy,
            });

            cache.ExistingDepartments.Add(name?.ToLower() ?? "");
            return (true, false, false, null);
        }
        else if (type == "Designation")
        {
            string name = row[0]?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            if (cache.ExistingDesignations.Contains(name?.ToLower() ?? ""))
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Designation", name, "Enter unique Designation name.", type));

            await _unitOfWork.DesignationRepository.AddAsync(new Designation
            {
                DesignationName = name,
                SortingNo = int.TryParse(row[1]?.ToString(), out int sortNo) ? sortNo : 0,
                IsEnabled = true,
                IsDeleted = false,
                CreatedBy = createdBy,
            });

            cache.ExistingDesignations.Add(name?.ToLower() ?? "");
            return (true, false, false, null);
        }
        else if (type == "Bank")
        {
            string Bankname = row[0]?.ToString()?.Trim();
            string code = row[1]?.ToString()?.Trim();
            string BranchName = row[2]?.ToString()?.Trim();
            string Accountno = row[3]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(Bankname) &&
                string.IsNullOrWhiteSpace(BranchName) && string.IsNullOrWhiteSpace(Accountno))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            if (cache.ExistingBanks.Contains(code?.ToLower() ?? ""))
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Bank Code", code, "Enter unique Bank Code.", type));

            await _unitOfWork.BankMasterRepository.AddAsync(new BankMaster
            {
                BankName = Bankname,
                BankCode = code,
                BranchName = BranchName,
                AccountNo = Accountno,
                Address = row[4]?.ToString(),
                City = row[5]?.ToString(),
                BankBSRCode = row[6]?.ToString(),
                IsDefaultBank = bool.TryParse(row[7]?.ToString(), out bool isDefault) ? isDefault : false,
                IsEnabled = true,
                IsDeleted = false,
                CreatedBy = createdBy,
            });

            cache.ExistingBanks.Add(code?.ToLower() ?? "");
            return (true, false, false, null);
        }
        else if (type == "City")
        {
            string cityname = row[0]?.ToString()?.Trim();
            string stateName = row[1]?.ToString()?.Trim();
            string Country = row[2]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(cityname) && string.IsNullOrWhiteSpace(stateName) &&
                string.IsNullOrWhiteSpace(Country))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            if (!cache.StateIdByName.TryGetValue(stateName?.ToLower() ?? "", out int stateId))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid State", stateName, "Enter Valid State name.", type));

            string cityKey = $"{cityname?.ToLower()}_{stateId}";
            if (cache.ExistingCities.Contains(cityKey))
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate city name", cityname, "Enter unique city name.", type));

            await _unitOfWork.CityRepository.AddAsync(new City
            {
                CityName = cityname,
                StateId = stateId,
                Country = Country,
                CityCategoryId = int.TryParse(row[3]?.ToString(), out int catId) ? catId : (int?)null,
                Remarks = row[4]?.ToString(),
                IsEnabled = true,
                IsDeleted = false,
                CreatedBy = createdBy,
            });

            cache.ExistingCities.Add(cityKey);
            return (true, false, false, null);
        }
        else if (type == "Holiday")
        {
            string name = row[0]?.ToString()?.Trim();
            string Branchname = row[1]?.ToString()?.Trim();
            DateTime fromDate = DateTime.TryParse(row[2]?.ToString(), out DateTime parsedDate) ? parsedDate : DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(Branchname))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            if (!cache.BranchIdByName.TryGetValue(Branchname?.ToLower() ?? "", out int branchId))
                return (false, true, false, CreateErrorRow(rowIndex, "Invalid Branch name", Branchname, "Enter Valid Branch Name", type));

            if (cache.ExistingHolidays.Contains(name?.ToLower() ?? ""))
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Holiday Name", name, "Enter unique Holiday Name.", type));

            // Uncomment to actually insert
            /*
            await _unitOfWork.HolidayMasterRepository.AddAsync(new HolidayMaster
            {
                HolidayName = name,
                BranchId = branchId,
                FromDate = fromDate,
                ToDate = DateTime.TryParse(row[3]?.ToString(), out DateTime toDate) ? toDate : DateTime.MinValue,
                Holidaycategory = row[4]?.ToString(),
                RepeatAnnually = bool.TryParse(row[5]?.ToString(), out bool repeat) ? repeat : false,
                HalfDay = bool.TryParse(row[6]?.ToString(), out bool halfDay) ? halfDay : false,
                PresentCompulsory = bool.TryParse(row[7]?.ToString(), out bool compulsory) ? compulsory : false,
                OptionalHoliday = bool.TryParse(row[8]?.ToString(), out bool optional) ? optional : false,
                ApprovalMaxLimit = row[9]?.ToString() ?? "0",
                IsEnabled = true,
                IsDeleted = false
            });
            */

            cache.ExistingHolidays.Add(name?.ToLower() ?? "");
            return (true, false, false, null);
        }

        return (false, false, false, CreateErrorRow(rowIndex, "Unknown Type", "", $"Unknown master type: {type}", type));
    }

    private object CreateErrorRow(int rowNumber, string errorType, string value, string message, string section)
    {
        return new
        {
            rowNumber = rowNumber,
            employeeCode = value,
            importDate = DateTime.Now.ToString("dd-MM-yyyy"),
            errorDescription = message,
            actualValue = value,
            suggestion = errorType,
            importType = section
        };
    }
}