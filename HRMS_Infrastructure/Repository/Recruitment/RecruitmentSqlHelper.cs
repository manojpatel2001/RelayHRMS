using HRMS_Utility;
using System.Collections.Generic;

namespace HRMS_Infrastructure.Repository.Recruitment
{
    // Shared helper for the Recruitment module's CRUD stored procedures, which all follow
    // the sp_EmployeeRecruitmentDetails_CRUD.sql convention: SELECT [NewId,] Success, ResponseMessage.
    internal static class RecruitmentSqlHelper
    {
        public static APIResponse ToApiResponse(dynamic row)
        {
            var response = new APIResponse();
            if (row == null)
            {
                response.isSuccess = false;
                response.ResponseMessage = "No response from database";
                return response;
            }

            var dict = (IDictionary<string, object>)row;
            int success = dict.ContainsKey("Success") && dict["Success"] != null ? System.Convert.ToInt32(dict["Success"]) : -1;
            response.isSuccess = success > 0;
            response.ResponseMessage = dict.ContainsKey("ResponseMessage") ? dict["ResponseMessage"]?.ToString() : null;
            response.Data = dict.ContainsKey("NewId") ? dict["NewId"] : null;
            return response;
        }
    }
}
