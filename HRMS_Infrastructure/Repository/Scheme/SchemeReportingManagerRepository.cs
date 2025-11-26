using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.Master.Scheme;
using HRMS_Infrastructure.Interface.Scheme;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Scheme
{
    public class SchemeReportingManagerRepository: ISchemeReportingManagerRepository
    {
        private readonly string _connectionString;
        private readonly HRMSDbContext _db;

        public SchemeReportingManagerRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> ManageSchemeReportingManagers(List<SchemeReportingManagerModel> models)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // ✅ Create DataTable matching SQL Type (SchemeReportingManagerType)
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("SchemeId", typeof(int));
                    dataTable.Columns.Add("CompanyId", typeof(int));
                    dataTable.Columns.Add("LevelNo", typeof(int));
                    dataTable.Columns.Add("ReportingManagerId", typeof(int));
                    dataTable.Columns.Add("DesignationId", typeof(int));

                    // Common Flags
                    dataTable.Columns.Add("IsHR", typeof(bool));
                    dataTable.Columns.Add("IsReportingManager", typeof(bool));
                    dataTable.Columns.Add("IsReportingToReportingManager", typeof(bool));
                    dataTable.Columns.Add("IsHOD", typeof(bool));
                    dataTable.Columns.Add("IsNotMandatory", typeof(bool));

                    // Probation-specific Flags
                    dataTable.Columns.Add("IsBranchManager", typeof(bool));
                    dataTable.Columns.Add("IsProbationManager", typeof(bool));

                    // Escalation Handling
                    dataTable.Columns.Add("EscalationDays", typeof(int));
                    dataTable.Columns.Add("NextLevelId", typeof(int));

                    // Audit Columns
                    dataTable.Columns.Add("IsEnabled", typeof(bool));
                    dataTable.Columns.Add("CreatedBy", typeof(string));
                    dataTable.Columns.Add("UpdatedBy", typeof(string));
                    dataTable.Columns.Add("DeletedBy", typeof(string));

                    // Operation
                    dataTable.Columns.Add("OperationType", typeof(string));

                    // ✅ Populate DataTable with safe defaults
                    foreach (var model in models)
                    {
                        dataTable.Rows.Add(
                            model.Id,
                            model.SchemeId,
                            model.CompanyId,
                            model.LevelNo,
                            model.ReportingManagerId,
                            model.DesignationId,

                            // Common Flags (default false)
                            model.IsHR ?? false,
                            model.IsReportingManager ?? false,
                            model.IsReportingToReportingManager ?? false,
                            model.IsHOD ?? false,
                            model.IsNotMandatory ?? false,

                            // Probation-specific Flags (default false)
                            model.IsBranchManager ?? false,
                            model.IsProbationManager ?? false,

                            // Escalation Handling
                            model.EscalationDays,
                            model.NextLevelId,

                            // Status and Audit (default true for IsEnabled)
                            model.IsEnabled == false ? false : true,
                            model.CreatedBy,
                            model.UpdatedBy,
                            model.DeletedBy,

                            // Operation Control
                            model.OperationType ?? "Create"
                        );
                    }

                    // ✅ Set parameters for stored procedure
                    var parameters = new DynamicParameters();
                    parameters.Add("@SchemeReportingManagers", dataTable.AsTableValuedParameter("SchemeReportingManagerType"));
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    // ✅ Execute stored procedure
                    await connection.ExecuteAsync(
                        "USP_Manage_SchemeReportingManager",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<APIResponse> GetSchemeDropdownDetails()
        {
            var response = new APIResponse();
            try
            {
                var result = new SchemeDropdownDetails
                {
                    Companies = new List<CompanyModel>(),
                    SchemeTypes = new List<HRMS_Core.Master.Scheme.SchemeTypeModel>(),
                    Designations = new List<DesignationModel>(),
                    Departments = new List<DepartmentModel>()
                };

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var multi = await connection.QueryMultipleAsync("GetSchemeDropdownDetails", commandType: CommandType.StoredProcedure))
                    {
                        result.Companies = (await multi.ReadAsync<CompanyModel>()).AsList();
                        result.SchemeTypes = (await multi.ReadAsync<HRMS_Core.Master.Scheme.SchemeTypeModel>()).AsList();
                        result.Designations = (await multi.ReadAsync<DesignationModel>()).AsList();
                        result.Departments = (await multi.ReadAsync<DepartmentModel>()).AsList();
                    }
                }

                response.isSuccess = true;
                response.ResponseMessage = "Dropdown details fetched successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
            }
            return response;
        }

        public async Task<APIResponse> GetDrpSchemeDetailsBySchemeType(int schemeTypeId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@SchemeTypeId", schemeTypeId);

                    var schemes = await connection.QueryAsync<SchemeModel>(
                        "GetDrpSchemeDetailsBySchemeType",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = true;
                    response.ResponseMessage = "Schemes fetched successfully.";
                    response.Data = schemes;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
            }
            return response;
        }

        public async Task<APIResponse> GetReportingByCompanyId(int companyId, int? designationId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", companyId);
                    parameters.Add("@DesignationId", designationId);

                    var employees = await connection.QueryAsync<EmployeeModel>(
                        "GetReportingByCompanyId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = true;
                    response.ResponseMessage = "Reporting managers fetched successfully.";
                    response.Data = employees;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
            }
            return response;
        }
        public async Task<APIResponse> GetAllEmployByDepartmentId(int? companyId, int? departmentId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", companyId);
                    parameters.Add("@DepartmentId", departmentId);

                    var employees = await connection.QueryAsync<EmployeeModel>(
                        "GetAllEmployByDepartmentId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = true;
                    response.ResponseMessage = "Reporting managers fetched successfully.";
                    response.Data = employees;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
            }
            return response;
        }

    }
}
