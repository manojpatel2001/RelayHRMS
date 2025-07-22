using Azure.Core;
using ExcelDataReader;
using HRMS_Core.ControlPanel.ImportData;
using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography.X509Certificates;
using System.Text;


[ApiController]
[Route("api/[controller]")]
public class ImportDataController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public ImportDataController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _unitOfWork = unitOfWork;
        _env = env;
    }
    [HttpPost("Upload")]
    public async Task<APIResponse> Upload([FromForm] ImportRequest request)
    {
        try
        {
            if (request.File == null || request.RowFrom <= 0 || request.RowTo < request.RowFrom)
                return new APIResponse { isSuccess = false, ResponseMessage = "Invalid input" };

            var errorRows = new List<object>();
            int insertedCount = 0, duplicateCount = 0, blankCount = 0;

            var extension = Path.GetExtension(request.File.FileName).ToLower();
            var dt = new DataTable();

            using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream);
            stream.Position = 0;

            if (extension == ".xlsx")
            {

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[request.SheetName];
                if (worksheet == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Sheet {request.SheetName} not found" };

                int colCount = worksheet.Dimension.End.Column;

                for (int col = 1; col <= colCount; col++)
                    dt.Columns.Add(worksheet.Cells[1, col].Text);

                for (int row = request.RowFrom; row <= request.RowTo; row++)
                {
                    var dataRow = dt.NewRow();
                    bool isRowEmpty = true;

                    for (int col = 1; col <= colCount; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Text?.Trim();
                        dataRow[col - 1] = cellValue;
                        if (!string.IsNullOrWhiteSpace(cellValue)) isRowEmpty = false;
                    }

                    if (!isRowEmpty)
                        dt.Rows.Add(dataRow);
                }
            }
            else if (extension == ".xls")
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using var reader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(stream);
                var result = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataReader.ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = false
                    }
                });

                var sheet = result.Tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == request.SheetName);
                if (sheet == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Sheet {request.SheetName} not found" };

                // Header (first row)
                for (int col = 0; col < sheet.Columns.Count; col++)
                    dt.Columns.Add(sheet.Rows[0][col]?.ToString());

                // Read data rows between RowFrom and RowTo
                for (int row = request.RowFrom - 1; row <= request.RowTo - 1 && row < sheet.Rows.Count; row++)
                {
                    var sourceRow = sheet.Rows[row];
                    var dataRow = dt.NewRow();
                    bool isRowEmpty = true;

                    for (int col = 0; col < sheet.Columns.Count; col++)
                    {
                        var value = sourceRow[col]?.ToString()?.Trim();
                        dataRow[col] = value;
                        if (!string.IsNullOrWhiteSpace(value))
                            isRowEmpty = false;
                    }

                    if (!isRowEmpty)
                        dt.Rows.Add(dataRow);
                }
            }
            else
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Only .xls and .xlsx files are supported." };
            }

            // ✅ Template column validation
            if (!ValidateTemplateColumns(dt, request.Type, out string headerError))
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = headerError
                };
            }

            // ✅ Row-wise processing
            foreach (DataRow row in dt.Rows)
            {
                int rowIndex = dt.Rows.IndexOf(row) + request.RowFrom;
                var result = await ProcessRowByType(row, request.Type, rowIndex);

                if (result.IsSuccess)
                    insertedCount++;
                else
                {
                    if (result.IsDuplicate) duplicateCount++;
                    if (result.IsBlank) blankCount++;
                    errorRows.Add(result.ErrorRow);
                }
            }

            await _unitOfWork.CommitAsync();

            string msg = $"{insertedCount} row(s) inserted.";
            if (duplicateCount > 0 || blankCount > 0)
                msg += $" {duplicateCount} duplicate(s), {blankCount} blank row(s) skipped.";

            return new APIResponse
            {
                isSuccess = insertedCount > 0,
                ResponseMessage = msg.Trim(),
                Data = errorRows.Any() ? errorRows : null
            };
        }
        catch (Exception ex)
        {
            return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
        }
    }



    private bool ValidateTemplateColumns(DataTable dt, string type, out string error)
    {
        error = "";
        List<string> expectedHeaders = new();

        switch (type)
        {
            case "Branch":
                expectedHeaders = new List<string> { "Branch_Code", "Branch_Name", "Branch_City", "Branch_Address", "Comp_Name", "State_Name", "Country_Name" };
                break;

            case "Department":
                expectedHeaders = new List<string> { "Department_Name", "Department_Disp_No" };
                break;

            case "Designation":
                expectedHeaders = new List<string> { "DesignationName", "DesignationCode" };
                break;

            case "Bank":
                expectedHeaders = new List<string> { "BankName", "BankCode", "BranchName", "AccountNo", "Address", "City", "BankBSRCode", "IsDefault" };
                break;

            case "City":
                expectedHeaders = new List<string> { "City_Name", "State_Name", "Country", "Category", "Entry_Type" };
                break;

            case "Holiday ":
                expectedHeaders = new List<string>
            {
               "Holiday_Name", "Branch_Name", "From_Date", "To_Date", "Holiday_Category", "Repeat_Annually", "Half_Day", "Present_Compulsory" , "Optional_Holiday", "Approval_Max_Limit", "Unpaid_Holiday"
            };
                break;

            case "MonthlyEar":
                expectedHeaders = new List<string> { "Alpha_Emp_code", "Month", "Year", "Basic", "HRA", "Conveyance", "Medical", "Deputation", "ChildEducationAllowance" };
                break;

            case "MonthlyDed":
                expectedHeaders = new List<string> { "Alpha_Emp_code", "Month", "Year","PF", "ESIC", "PT", "LWF","TDS" , "TermInsurance" , "GroupMedical" };
                break;

            case "Attendance":
                expectedHeaders = new List<string> { "Alpha Emp Code", "Month", "Year", "1", "2", "3", "4", "5", "6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31" };
                break;

            default:
                error = $"No template defined for type '{type}'";
                return false;
        }

        // Validate headers from DataTable
        for (int i = 0; i < expectedHeaders.Count; i++)
        {
            if (dt.Columns.Count <= i || dt.Columns[i].ColumnName.Trim() != expectedHeaders[i].Trim())
            {
                error = $"Template mismatch: Column {i + 1} must be '{expectedHeaders[i]}', but found '{dt.Columns[i].ColumnName}'.";
                return false;
            }
        }

        return true;
    }

    private async Task<(bool IsSuccess, bool IsDuplicate, bool IsBlank, object ErrorRow)> ProcessRowByType(DataRow row, string type, int rowIndex)
    {
        if (type == "Branch")
        {
            string code = row[0]?.ToString()?.Trim();
            string name = row[1]?.ToString()?.Trim();
            string city = row[2]?.ToString()?.Trim();
            string state = row[5]?.ToString()?.Trim();
            string country = row[6]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(city) &&
                string.IsNullOrWhiteSpace(state) && string.IsNullOrWhiteSpace(country))
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            }

            var exists = await _unitOfWork.BranchRepository.GetAsync(x => x.BranchCode == code && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Branch Code", code, "Enter unique Branch Code.", type));

            await _unitOfWork.BranchRepository.AddAsync(new Branch
            {
                BranchCode = code,
                BranchName = name,
                CityName = city,
                Address = row[3]?.ToString(),
                CompanyName = row[4]?.ToString(),
                State = state,
                CountryName = country,
                IsEnabled = true,
                IsDeleted = false
            });

            return (true, false, false, null);
        }

        else if (type == "Department")
        {
            string name = row[0]?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));

            var exists = await _unitOfWork.DepartmentRepository.GetAsync(x => x.DepartmentName == name && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Department", name, "Enter unique Department name.", type));

            await _unitOfWork.DepartmentRepository.AddAsync(new Department
            {
                DepartmentName = name,
                SortingNo = int.TryParse(row[1]?.ToString(), out int sortNo) ? sortNo : 0,
                IsEnabled = true,
                IsDeleted = false
            });

            return (true, false, false, null);
        }
        else if (type == "Designation")


        {
            string name = row[0]?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            var exists = await _unitOfWork.DesignationRepository.GetAsync(x => x.DesignationName == name && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Department", name, "Enter unique Department name.", type));

            await _unitOfWork.DesignationRepository.AddAsync(new Designation
            {
                DesignationName = name,
                SortingNo = int.TryParse(row[1]?.ToString(), out int sortNo) ? sortNo : 0,
                IsEnabled = true,
                IsDeleted = false
            });
            return (true, false, false, null);
        }
        else if (type == "Bank")
        {
            string Bankname = row[0]?.ToString().Trim();
            string code = row[1]?.ToString().Trim();
            string BranchName = row[2]?.ToString().Trim();
            string Accountno = row[3]?.ToString().Trim();

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(Bankname)
                && string.IsNullOrWhiteSpace(BranchName) && string.IsNullOrWhiteSpace(Accountno))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            var exists = await _unitOfWork.BankMasterRepository.GetAsync(x => x.BankCode == code && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
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
                IsDeleted = false
            });
            return (true, false, false, null);
        }
        else if (type == "City")
        {
            string cityname = row[0]?.ToString().Trim();
            string stateName = row[1]?.ToString()?.Trim();

            string Country = row[2]?.ToString().Trim();// City name

            if (string.IsNullOrWhiteSpace(cityname) && string.IsNullOrWhiteSpace(stateName)
           && string.IsNullOrWhiteSpace(Country))
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            var state = await _unitOfWork.StateRepository.GetAsync(x => x.StateName.ToLower() == stateName.ToLower() && x.IsEnabled == true && x.IsDeleted != true);
       
            if (state == null)
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid State", "", "", type));

            }
            var exists = await _unitOfWork.CityRepository.GetAsync(x => x.CityName == cityname && x.StateId == state.StateId && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
            {
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Branch Code", cityname, "Enter unique Branch Code.", type));

            }
            await _unitOfWork.CityRepository.AddAsync(new City
            {
                CityName = cityname,
                StateId = state.StateId,
                Country = Country,
                CityCategoryId = int.TryParse(row[3]?.ToString(), out int catId) ? catId : (int?)null,
                Remarks = row[4]?.ToString(),
                IsEnabled = true,
                IsDeleted = false
            });

            return (true, false, false, null);
        }

        else if (type == "Holiday Master")
        {
            string name = row[0]?.ToString().Trim();
            DateTime fromDate = DateTime.TryParse(row[2]?.ToString(), out DateTime parsedDate) ? parsedDate : DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(name) && fromDate == DateTime.MinValue)
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            }
            var exists = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayName == name && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
            {
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate Holiday Name", name, "Enter unique Holiday Name.", type));

            }

            await _unitOfWork.HolidayMasterRepository.AddAsync(new HolidayMaster
            {
                HolidayName = name,
                FromDate = fromDate,
                BranchId = int.TryParse(row[2]?.ToString(), out int branchId) ? branchId : 0,
                ToDate = DateTime.TryParse(row[5]?.ToString(), out DateTime toDate) ? toDate : DateTime.MinValue,
                Holidaycategory = row[7]?.ToString(),
                RepeatAnnually = bool.TryParse(row[8]?.ToString(), out bool repeat) ? repeat : false,
                HalfDay = bool.TryParse(row[9]?.ToString(), out bool halfDay) ? halfDay : false,
                PresentCompulsory = bool.TryParse(row[10]?.ToString(), out bool compulsory) ? compulsory : false,
                OptionalHoliday = bool.TryParse(row[12]?.ToString(), out bool optional) ? optional : false,
                ApprovalMaxLimit = row[13]?.ToString() ?? "0",
                IsEnabled = true,
                IsDeleted = false
            });

            return (true, false, false, null);
        }
        else if (type == "MonthlyEar")

        {
            string codeStr = row[0]?.ToString().Trim();
            string monthStr = row[1]?.ToString().Trim();
            string yearStr = row[2]?.ToString().Trim();

            if (string.IsNullOrWhiteSpace(codeStr) &&
                string.IsNullOrWhiteSpace(monthStr) &&
                string.IsNullOrWhiteSpace(yearStr))
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            }

            if (!int.TryParse(codeStr, out int code))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Code", codeStr, "Code must be a valid number", type));

            if (!int.TryParse(monthStr, out int month))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Month", monthStr, "Month must be a valid number", type));

            if (!int.TryParse(yearStr, out int year))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Year", yearStr, "Year must be a valid number", type));


            var exists = await _unitOfWork.EarningRepository.GetAsync(x => x.EmployeeId == code && x.Month == month && x.Year == year && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
            {
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate  Employee code", codeStr, "Enter unique Employee code.", type));

            }
            await _unitOfWork.EarningRepository.AddAsync(new Earning
            {
                EmployeeId = code,
                Month = month,
                Basic = decimal.TryParse(row[3]?.ToString(), out decimal basic) ? basic : (decimal?)null,
                HRA = decimal.TryParse(row[4]?.ToString(), out decimal hra) ? hra : (decimal?)null,
                Conveyance = decimal.TryParse(row[5]?.ToString(), out decimal conveyance) ? conveyance : (decimal?)null,
                Medical = decimal.TryParse(row[6]?.ToString(), out decimal medical) ? medical : (decimal?)null,
                Deputation = decimal.TryParse(row[7]?.ToString(), out decimal deputation) ? deputation : (decimal?)null,
                ChildEducationAllowance = decimal.TryParse(row[8]?.ToString(), out decimal ChildEducationAllowance) ? ChildEducationAllowance : (decimal?)null,
                Year = year,
                IsEnabled = true,
                IsDeleted = false
            });
            return (true, false, false, null);
        }
        else if (type == "MonthlyDed")

        {
            string codeStr = row[0]?.ToString().Trim();
            string monthStr = row[1]?.ToString().Trim();
            string yearStr = row[2]?.ToString().Trim();

            if (string.IsNullOrWhiteSpace(codeStr) &&
                string.IsNullOrWhiteSpace(monthStr) &&
                string.IsNullOrWhiteSpace(yearStr))
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            }

            if (!int.TryParse(codeStr, out int code))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Code", codeStr, "Code must be a valid number", type));

            if (!int.TryParse(monthStr, out int month))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Month", monthStr, "Month must be a valid number", type));

            if (!int.TryParse(yearStr, out int year))
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Year", yearStr, "Year must be a valid number", type));


            var exists = await _unitOfWork.DeductionRepository.GetAsync(x => x.EmployeeId == code && x.Month == month && x.Year == year && x.IsEnabled == true && x.IsDeleted != true);
            if (exists != null)
            {
                return (false, true, false, CreateErrorRow(rowIndex, "Duplicate  Employee code", codeStr, "Enter unique Employee code.", type));

            }


            await _unitOfWork.DeductionRepository.AddAsync(new Deduction
            {
                EmployeeId = code,
                Month = month,
                Year = year,
                PF = decimal.TryParse(row[3]?.ToString(), out decimal pf) ? pf : (decimal?)null,
                ESIC = decimal.TryParse(row[4]?.ToString(), out decimal esic) ? esic : (decimal?)null,
                PT = decimal.TryParse(row[5]?.ToString(), out decimal pt) ? pt : (decimal?)null,
                LWF = decimal.TryParse(row[6]?.ToString(), out decimal lwf) ? lwf : (decimal?)null,
                TDS = decimal.TryParse(row[7]?.ToString(), out decimal tds) ? tds : (decimal?)null,
                TermInsurance = decimal.TryParse(row[8]?.ToString(), out decimal TermInsurance) ? TermInsurance : (decimal?)null,
                GroupMedical = decimal.TryParse(row[9]?.ToString(), out decimal GroupMedical) ? GroupMedical : (decimal?)null,
                IsEnabled = true,
                IsDeleted = false
            });
            return (true, false, false, null);
        }
        else if (type == "Attendance")
        {
            string empStr = row[0]?.ToString()?.Trim();
            string monthStr = row[1]?.ToString()?.Trim();
            string yearStr = row[2]?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(empStr) &&
                string.IsNullOrWhiteSpace(monthStr) &&
                string.IsNullOrWhiteSpace(yearStr))
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            }

            if (!int.TryParse(monthStr, out int month))
            { return (false, false, true, CreateErrorRow(rowIndex, "Invalid Month", monthStr, "Month must be a valid number", type)); }
            if (string.IsNullOrWhiteSpace(yearStr) || !int.TryParse(yearStr, out int year))
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Year", yearStr, "Year must be a valid number", type));
            }

            var Epl = await _unitOfWork.EmployeeManageRepository.GetAsync(x => x.EmployeeCode == empStr && x.IsEnabled == true && x.IsDeleted != true);
            if (Epl == null)
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Emp_Code", empStr, "Employee not found or inactive", type));
            }

            List<string> dailyAttendance = new();
            for (int i = 4; i < 34; i++) // 4 to 33 = 30 days
            {
                string val = row[i]?.ToString()?.Trim().ToUpper() ?? "";
                dailyAttendance.Add(val);
            }

            string attDetail = string.Join(",", dailyAttendance);

            await _unitOfWork.EmpAttendanceRepository.AddAsync(new EmpAttendanceImport
            {
                Emp_ID = Epl.Id,
                Month = month,
                Year = year,
                Att_Detail = attDetail,
                IsEnabled = true,
                IsDeleted = false
            });

            return (true, false, false, null);
        }
        else if (type == "LeaveOpening")
        {
            string empStr = row[0]?.ToString()?.Trim();         // EmployeeCode
            string leaveStr = row[1]?.ToString()?.Trim();       // Leave Name or LeaveId
            // Blank Row Check
            if (string.IsNullOrWhiteSpace(empStr) &&
                string.IsNullOrWhiteSpace(leaveStr) )
         
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Blank row", "", "Row appears empty", type));
            }

            var emp = await _unitOfWork.EmployeeManageRepository.GetAsync(x => x.EmployeeCode == empStr && x.IsEnabled == true && x.IsDeleted != true);
            if (emp == null)
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid Emp_Code", empStr, "Employee not found or inactive", type));
            }

            var Leave = await _unitOfWork.LeaveMasterRepository.GetAsync(x => x.Leave_Type.ToLower() == leaveStr.ToLower() && x.IsEnabled == true && x.IsDeleted != true);

            if (Leave == null)
            {
                return (false, false, true, CreateErrorRow(rowIndex, "Invalid State", "", "", type));

            }

            await _unitOfWork.LeaveOpeningRepository.AddAsync(new LeaveOpening
            {
                EMP_Id = emp.Id,
                LeaveId = Leave.Leave_TypeId,
                Opening = decimal.TryParse(row[2]?.ToString(), out decimal pf) ? pf : (decimal?)null,
                Grade = row[3]?.ToString(),
                EffectiveDate = DateTime.TryParse(row[4]?.ToString(), out DateTime effDate) ? effDate : (DateTime?)null,          
                comp_id = emp.CompanyId,
                IsEnabled = true,
                IsDeleted = false,
            });

            return (true, false, false, null);
        }
        return (false, false, false, CreateErrorRow(rowIndex, "Unknown Type", "", $"Unknown master type: {type}", type));
    }
    private object CreateErrorRow(int rowNumber, string errorType, string value, string message, string section)
    {
        return new
        {
            rowNumber = rowNumber,                  // maps to "Row Number"
            employeeCode = value,                   // shows the value that caused error (e.g., emp code, branch code)
            importDate = DateTime.Now.ToString("dd-MM-yyyy"), // static current date
            errorDescription = message,             // message shown in Error Description column
            actualValue = value,                    // same as employeeCode for now
            suggestion = errorType,                 // show type of issue like "Duplicate" etc.
            importType = section                    // like "Branch", "MonthlyDed" etc.
        };
    }



}
