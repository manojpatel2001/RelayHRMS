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

            // Save the file temporarily
            string uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            string filePath = Path.Combine(uploadsFolder, request.File.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                request.File.CopyTo(fileStream);
            }

            DataTable dt = ReadExcel(filePath, request.SheetName, request.RowFrom, request.RowTo);
            if (dt == null || dt.Rows.Count == 0)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "No data found." };
            }

            var errorRows = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                int rowIndex = dt.Rows.IndexOf(row) + request.RowFrom;

                if (request.Type == "Branch")
                {
                    string branchCode = row[0].ToString();
                    var existingBranch = await _unitOfWork.BranchRepository
                     .GetAsync(x => x.BranchCode == branchCode
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same Branch Code is exists", branchCode, "Please Enter valid branch code :(Branch Code :" + branchCode + ")", "Branch Master"));

                    }
                    else
                    {

                        var branch = new Branch
                        {
                            BranchCode = branchCode,
                            BranchName = row[1].ToString(),
                            CityName = row[2].ToString(),
                            Address = row[3].ToString(),
                            CompanyName = row[4].ToString(),
                            State = row[5].ToString(),
                            CountryName = row[6].ToString(),
                            IsEnabled = true,
                            IsDeleted = false
                        };

                        await _unitOfWork.BranchRepository.AddAsync(branch);
                    }

                }
                else if (request.Type == "Department")

                {
                    string DepartmentName = row[0].ToString();
                    var existingBranch = await _unitOfWork.DepartmentRepository
                     .GetAsync(x => x.DepartmentName == DepartmentName
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same Department Code is exists", DepartmentName, "Please Enter valid DepartmentName :(DepartmentName :" + DepartmentName + ")", "Department Master"));

                    }
                    else
                    {
                        var dept = new Department
                        {
                            DepartmentName = row[0].ToString(),
                            Code = row[1].ToString(),
                            SortingNo = int.TryParse(row[2].ToString(), out int sorting) ? sorting : 0,
                            OJTApplicable = row[3].ToString().ToLower() == "true" || row[3].ToString() == "1"
                        };

                        await _unitOfWork.DepartmentRepository.AddAsync(dept);
                    }
                }
                else if (request.Type == "Designation")
                {

                    string DesignationCode = row[0].ToString();
                    var existingBranch = await _unitOfWork.DesignationRepository
                     .GetAsync(x => x.DesignationCode == DesignationCode
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same Designation Code is exists", DesignationCode, "Please Enter valid Designation :(DesignationCode :" + DesignationCode + ")", "Designation Master"));

                    }
                    else
                    {
                        var desg = new Designation
                        {
                            DesignationName = row[0].ToString(),
                            DesignationCode = row[1].ToString(),
                            SortingNo = int.TryParse(row[2].ToString(), out int sorting) ? sorting : 0,



                        };
                        await _unitOfWork.DesignationRepository.AddAsync(desg);
                    }
                }
                else if (request.Type == "Bank")
                {
                    string Bank = row[0].ToString();
                    var existingBranch = await _unitOfWork.BankMasterRepository
                     .GetAsync(x => x.BankCode == Bank
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same Bank is exists", Bank, "Please Enter valid Bank :(Bank :" + Bank + ")", "Bank Master"));

                    }

                    else
                    {
                        var bank = new BankMaster
                        {
                            BankName = row[0].ToString(),
                            BankCode = row[1].ToString(),
                            BranchName = row[2].ToString(),
                            AccountNo = row[3].ToString(),
                            Address = row[4].ToString(),
                            City = row[5].ToString(),
                            BankBSRCode = row[6].ToString(),
                            IsDefaultBank = bool.TryParse(row[7]?.ToString(), out bool isDefault) ? isDefault : false
                        };

                        await _unitOfWork.BankMasterRepository.AddAsync(bank);
                    }
                }
                else if (request.Type == "City Master")
                {

                    string citymaster = row[0].ToString();
                    var existingBranch = await _unitOfWork.CityRepository
                     .GetAsync(x => x.CityName == citymaster
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same city name is exists", citymaster, "Please Enter valid city name :(city name:" + citymaster + ")", "city Master"));

                    }
                    else
                    {

                        var city = new City
                        {
                            CityName = row[0]?.ToString(),
                            StateId = int.TryParse(row[1]?.ToString(), out int stateId) ? stateId : (int?)null,
                            Country = row[2]?.ToString(),
                            CityCategoryId = int.TryParse(row[3]?.ToString(), out int categoryId) ? categoryId : (int?)null,
                            Remarks = row[4]?.ToString()
                        };

                        await _unitOfWork.CityRepository.AddAsync(city);
                    }
                }
                else if (request.Type == "Holiday Master")
                {

                    string holidaymaster = row[0].ToString();
                    var existingBranch = await _unitOfWork.HolidayMasterRepository
                     .GetAsync(x => x.HolidayName == holidaymaster
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same holiday name is exists", holidaymaster, "Please Enter valid holiday name :(holiday name:" + holidaymaster + ")", "holiday Master"));

                    }
                    var holiday = new HolidayMaster
                    {
                        HolidayName = row[0]?.ToString(),
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
                        ApprovalMaxLimit = row[13]?.ToString() ?? "0"
                    };

                    await _unitOfWork.HolidayMasterRepository.AddAsync(holiday);
                }
                else if (request.Type == "Employee")
                {

                    string EmployeeCode = row[0].ToString();
                    var existingBranch = await _unitOfWork.EmployeeManageRepository
                     .GetAsync(x => x.EmployeeCode == EmployeeCode
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same Employee Code is exists", EmployeeCode, "Please Enter valid EmployeeCode :(EmployeeCode:" + EmployeeCode + ")", "Employee Master"));

                    }
                    var Employee = new HRMSUserIdentity
                    {
                        FullName = row[0]?.ToString(),
                        Initial = row[1]?.ToString(),
                        FirstName = row[2]?.ToString(),
                        MiddleName = row[3]?.ToString(),
                        LastName = row[4]?.ToString(),
                        EmployeeCode = row[5]?.ToString(),
                        DateOfJoining = DateTime.TryParse(row[6]?.ToString(), out DateTime doj) ? doj : DateTime.MinValue,
                        BranchId = int.TryParse(row[7]?.ToString(), out int branchId) ? branchId : 0,
                        GradeId = int.TryParse(row[8]?.ToString(), out int gradeId) ? gradeId : 0,
                        Shift = row[9]?.ToString(),
                        CTC = row[10]?.ToString(),
                        DesignationId = int.TryParse(row[11]?.ToString(), out int desigId) ? desigId : 0,
                        GrossSalary = decimal.TryParse(row[12]?.ToString(), out decimal gross) ? gross : 0,
                        Category = row[13]?.ToString(),
                        BasicSalary = decimal.TryParse(row[14]?.ToString(), out decimal basic) ? basic : 0,
                        DepartmentId = int.TryParse(row[15]?.ToString(), out int deptId) ? deptId : 0,
                        EmployeeType = row[16]?.ToString(),
                        DateOfBirth = DateTime.TryParse(row[17]?.ToString(), out DateTime dob) ? dob : DateTime.MinValue,
                        UserPrivilege = int.TryParse(row[18]?.ToString(), out int privilege) ? privilege : 0,
                        LoginAlias = row[19]?.ToString(),
                        Password = row[20]?.ToString(),
                        ReportingManager = row[21]?.ToString(),
                        SubBranch = row[22]?.ToString(),
                        EnrollNo = row[23]?.ToString(),
                        CompanyId = int.TryParse(row[24]?.ToString(), out int compId) ? compId : 0,
                        Overtime = bool.TryParse(row[25]?.ToString(), out bool ot) ? ot : false,
                        Latemark = bool.TryParse(row[26]?.ToString(), out bool late) ? late : false,
                        Earlymark = bool.TryParse(row[27]?.ToString(), out bool early) ? early : false,
                        Fullpf = bool.TryParse(row[28]?.ToString(), out bool pf) ? pf : false,
                        Pt = bool.TryParse(row[29]?.ToString(), out bool pt) ? pt : false,
                        Fixsalary = bool.TryParse(row[30]?.ToString(), out bool fix) ? fix : false,
                        Probation = bool.TryParse(row[31]?.ToString(), out bool probation) ? probation : false,
                        Trainee = bool.TryParse(row[32]?.ToString(), out bool trainee) ? trainee : false,
                        EmployeeProfileUrl = row[33]?.ToString(),
                        EmployeeSignatureUrl = row[34]?.ToString()
                    };
                    await _unitOfWork.EmployeeManageRepository.AddAsync(Employee);
                }
                else if (request.Type == "Employee")
                {

                    string EmployeeCode = row[0].ToString();
                    var existingBranch = await _unitOfWork.EmployeeManageRepository
                     .GetAsync(x => x.EmployeeCode == EmployeeCode
                        && x.IsEnabled == true
                        && x.IsDeleted != true);


                    if (existingBranch != null)
                    {
                        errorRows.Add(CreateErrorRow(rowIndex, "Same Employee Code is exists", EmployeeCode, "Please Enter valid EmployeeCode :(EmployeeCode:" + EmployeeCode + ")", "Employee Master"));

                    }
                    var Employee = new HRMSUserIdentity
                    {
                        FullName = row[0]?.ToString(),
                        Initial = row[1]?.ToString(),
                        FirstName = row[2]?.ToString(),
                        MiddleName = row[3]?.ToString(),
                        LastName = row[4]?.ToString(),
                        EmployeeCode = row[5]?.ToString(),
                        DateOfJoining = DateTime.TryParse(row[6]?.ToString(), out DateTime doj) ? doj : DateTime.MinValue,
                        BranchId = int.TryParse(row[7]?.ToString(), out int branchId) ? branchId : 0,
                        GradeId = int.TryParse(row[8]?.ToString(), out int gradeId) ? gradeId : 0,
                        Shift = row[9]?.ToString(),
                        CTC = row[10]?.ToString(),
                        DesignationId = int.TryParse(row[11]?.ToString(), out int desigId) ? desigId : 0,
                        GrossSalary = decimal.TryParse(row[12]?.ToString(), out decimal gross) ? gross : 0,
                        Category = row[13]?.ToString(),
                        BasicSalary = decimal.TryParse(row[14]?.ToString(), out decimal basic) ? basic : 0,
                        DepartmentId = int.TryParse(row[15]?.ToString(), out int deptId) ? deptId : 0,
                        EmployeeType = row[16]?.ToString(),
                        DateOfBirth = DateTime.TryParse(row[17]?.ToString(), out DateTime dob) ? dob : DateTime.MinValue,
                        UserPrivilege = int.TryParse(row[18]?.ToString(), out int privilege) ? privilege : 0,
                        LoginAlias = row[19]?.ToString(),
                        Password = row[20]?.ToString(),
                        ReportingManager = row[21]?.ToString(),
                        SubBranch = row[22]?.ToString(),
                        EnrollNo = row[23]?.ToString(),
                        CompanyId = int.TryParse(row[24]?.ToString(), out int compId) ? compId : 0,
                        Overtime = bool.TryParse(row[25]?.ToString(), out bool ot) ? ot : false,
                        Latemark = bool.TryParse(row[26]?.ToString(), out bool late) ? late : false,
                        Earlymark = bool.TryParse(row[27]?.ToString(), out bool early) ? early : false,
                        Fullpf = bool.TryParse(row[28]?.ToString(), out bool pf) ? pf : false,
                        Pt = bool.TryParse(row[29]?.ToString(), out bool pt) ? pt : false,
                        Fixsalary = bool.TryParse(row[30]?.ToString(), out bool fix) ? fix : false,
                        Probation = bool.TryParse(row[31]?.ToString(), out bool probation) ? probation : false,
                        Trainee = bool.TryParse(row[32]?.ToString(), out bool trainee) ? trainee : false,
                        EmployeeProfileUrl = row[33]?.ToString(),
                        EmployeeSignatureUrl = row[34]?.ToString()
                    };
                    await _unitOfWork.EmployeeManageRepository.AddAsync(Employee);
                }

            }

            if (errorRows.Any())
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Data is not importing due to error. ({errorRows.Count}) Record is invalid.",
                    Data = errorRows
                };
            }

            await _unitOfWork.CommitAsync();
            return new APIResponse
            {
                isSuccess = true,
                ResponseMessage = $"{request.Type} Imported Successfully"
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
