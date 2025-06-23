using HRMS_Core.ControlPanel.ImportData;
using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.OleDb;

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

            string uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            string filePath = Path.Combine(uploadsFolder, request.File.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                request.File.CopyTo(fileStream);
            }

            DataTable dt = ReadExcel(filePath, request.SheetName, request.RowFrom, request.RowTo);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            if (dt == null || dt.Rows.Count == 0)
                return new APIResponse { isSuccess = false, ResponseMessage = "No data found." };

            var errorRows = new List<object>();
            int insertedCount = 0;
            int duplicateCount = 0;
            int blankCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                int rowIndex = dt.Rows.IndexOf(row) + request.RowFrom;

                // Common vars
         
                if (request.Type == "Branch")

                {

                    string code = row[0]?.ToString().Trim();
                    string name = row[1]?.ToString().Trim();

                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        blankCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Blank row", "", "This row appears to be empty.", request.Type));
                        continue;
                    }

                    var exists = await _unitOfWork.BranchRepository.GetAsync(x => x.BranchCode == code && x.IsEnabled == true && x.IsDeleted != true);
                    if (exists != null)
                    {
                        duplicateCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Duplicate Branch Code", code, "Enter unique Branch Code.", "Branch Master"));
                        continue;
                    }

                    await _unitOfWork.BranchRepository.AddAsync(new Branch
                    {
                        BranchCode = code,
                        BranchName = name,
                        CityName = row[2]?.ToString(),
                        Address = row[3]?.ToString(),
                        CompanyName = row[4]?.ToString(),
                        State = row[5]?.ToString(),
                        CountryName = row[6]?.ToString(),
                        IsEnabled = true,
                        IsDeleted = false
                    });

                    insertedCount++;
                }

                else if (request.Type == "Department")

                {
                    string name = row[0]?.ToString().Trim();
                    string code = row[1]?.ToString().Trim();

                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        blankCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Blank row", "", "This row appears to be empty.", request.Type));
                        continue;
                    }

                    var exists = await _unitOfWork.DepartmentRepository.GetAsync(x => x.DepartmentName == name && x.IsEnabled == true && x.IsDeleted != true);
                    if (exists != null)
                    {
                        duplicateCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Duplicate Department", name, "Enter unique Department name.", "Department Master"));
                        continue;
                    }

                    await _unitOfWork.DepartmentRepository.AddAsync(new Department
                    {
                        DepartmentName = name,
                        Code = code,
                        SortingNo = int.TryParse(row[2]?.ToString(), out int sorting) ? sorting : 0,
                        OJTApplicable = row[3]?.ToString()?.ToLower() == "true" || row[3]?.ToString() == "1",
                        IsEnabled = true,
                        IsDeleted = false
                    });

                    insertedCount++;
                }

                else if (request.Type == "Designation")
 

                {

                    string code = row[0]?.ToString().Trim();
                    string name = row[1]?.ToString().Trim();

                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        blankCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Blank row", "", "This row appears to be empty.", request.Type));
                        continue;
                    }
                    var exists = await _unitOfWork.DesignationRepository.GetAsync(x => x.DesignationCode == code && x.IsEnabled == true && x.IsDeleted != true);
                    if (exists != null)
                    {
                        duplicateCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Duplicate Designation", code, "Enter unique Designation code.", "Designation Master"));
                        continue;
                    }

                    await _unitOfWork.DesignationRepository.AddAsync(new Designation
                    {
                        DesignationName = name,
                        DesignationCode = code,
                        SortingNo = int.TryParse(row[2]?.ToString(), out int sortNo) ? sortNo : 0,
                        IsEnabled = true,
                        IsDeleted = false
                    });

                    insertedCount++;
                }

                else if (request.Type == "Bank")
                {
                    string code = row[0]?.ToString().Trim();
                    string name = row[1]?.ToString().Trim();

                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        blankCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Blank row", "", "This row appears to be empty.", request.Type));
                        continue;
                    }
                    var exists = await _unitOfWork.BankMasterRepository.GetAsync(x => x.BankCode == code && x.IsEnabled == true && x.IsDeleted != true);
                    if (exists != null)
                    {
                        duplicateCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Duplicate Bank Code", code, "Enter unique Bank code.", "Bank Master"));
                        continue;
                    }

                    await _unitOfWork.BankMasterRepository.AddAsync(new BankMaster
                    {
                        BankName = name,
                        BankCode = code,
                        BranchName = row[2]?.ToString(),
                        AccountNo = row[3]?.ToString(),
                        Address = row[4]?.ToString(),
                        City = row[5]?.ToString(),
                        BankBSRCode = row[6]?.ToString(),
                        IsDefaultBank = bool.TryParse(row[7]?.ToString(), out bool isDefault) ? isDefault : false,
                        IsEnabled = true,
                        IsDeleted = false
                    });

                    insertedCount++;
                }

                else if (request.Type == "City")
                {
                    string code = row[0]?.ToString().Trim();
                    string name = row[1]?.ToString().Trim();

                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        blankCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Blank row", "", "This row appears to be empty.", request.Type));
                        continue;
                    }
                    var exists = await _unitOfWork.CityRepository.GetAsync(x => x.CityName == code && x.IsEnabled == true && x.IsDeleted != true);
                    if (exists != null)
                    {
                        duplicateCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Duplicate City", code, "Enter unique City name.", "City Master"));
                        continue;
                    }

                    await _unitOfWork.CityRepository.AddAsync(new City
                    {
                        CityName = code,
                        StateId = int.TryParse(row[1]?.ToString(), out int stateId) ? stateId : (int?)null,
                        Country = row[2]?.ToString(),
                        CityCategoryId = int.TryParse(row[3]?.ToString(), out int catId) ? catId : (int?)null,
                        Remarks = row[4]?.ToString(),
                        IsEnabled = true,
                        IsDeleted = false
                    });

                    insertedCount++;
                }

                else if (request.Type == "Holiday Master")
                {
                    string code = row[0]?.ToString().Trim();
                    string name = row[1]?.ToString().Trim();

                    if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    {
                        blankCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Blank row", "", "This row appears to be empty.", request.Type));
                        continue;
                    }
                    var exists = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayName == code && x.IsEnabled == true && x.IsDeleted != true);
                    if (exists != null)
                    {
                        duplicateCount++;
                        errorRows.Add(CreateErrorRow(rowIndex, "Duplicate Holiday Name", code, "Enter unique Holiday name.", "Holiday Master"));
                        continue;
                    }

                    await _unitOfWork.HolidayMasterRepository.AddAsync(new HolidayMaster
                    {
                        HolidayName = code,
                        State = row[1]?.ToString(),
                        BranchId = int.TryParse(row[2]?.ToString(), out int branchId) ? branchId : 0,
                        MultipleHoliday = bool.TryParse(row[3]?.ToString(), out bool multiple) ? multiple : false,
                        FromDate = DateTime.TryParse(row[4]?.ToString(), out DateTime fromDate) ? fromDate : DateTime.MinValue,
                        ToDate = DateTime.TryParse(row[5]?.ToString(), out DateTime toDate) ? toDate : DateTime.MinValue,
                        MessageText = row[6]?.ToString(),
                        Holidaycategory = row[7]?.ToString(),
                        RepeatAnnually = bool.TryParse(row[8]?.ToString(), out bool repeat) ? repeat : false,
                        HalfDay = bool.TryParse(row[9]?.ToString(), out bool halfDay) ? halfDay : false,
                        PresentCompulsory = bool.TryParse(row[10]?.ToString(), out bool compulsory) ? compulsory : false,
                        SMS = bool.TryParse(row[11]?.ToString(), out bool sms) ? sms : false,
                        OptionalHoliday = bool.TryParse(row[12]?.ToString(), out bool optional) ? optional : false,
                        ApprovalMaxLimit = row[13]?.ToString() ?? "0",
                        IsEnabled = true,
                        IsDeleted = false
                    });

                    insertedCount++;
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


    private DataTable ReadExcel(string filePath, string sheetName, int rowFrom, int rowTo)
    {
        string ext = Path.GetExtension(filePath);
        string connStr = "";

        if (ext == ".xls")
        {
            connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties='Excel 8.0;HDR=YES;'";
        }
        else if (ext == ".xlsx")
        {
            connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Xml;HDR=YES;'";
        }

        using (OleDbConnection conn = new OleDbConnection(connStr))
        {
            conn.Open();
            string query = $"SELECT * FROM [{sheetName}$]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            DataTable fullTable = new DataTable();
            adapter.Fill(fullTable);

            // Filter only required rows
            var filtered = fullTable.AsEnumerable()
                                    .Skip(rowFrom - 2) // Skip headers and start from rowFrom
                                    .Take(rowTo - rowFrom + 1);
            return filtered.CopyToDataTable();
        }
    }

    private object CreateErrorRow( int rowIndex, string error, string actualValue, string suggestion, string importType)
    {
        return new
        {
            rowNumber = rowIndex,
            employeeCode = "",
            importDate = DateTime.Now.ToString("yyyy-MM-dd"),
            errorDescription = error,
            actualValue = actualValue,
            suggestion = suggestion,
            importType = importType
        };
    }

}
