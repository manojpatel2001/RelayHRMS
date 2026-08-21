using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IBirthdayWishRepository
    {
        Task<VMCommonResult> CreateBirthdayWish(vmCreateBirthdayWish model);
        Task<VMCommonResult> DeleteBirthdayWish(int birthdayWishId, int wishedByEmployeeId);
        Task<List<vmBirthdayWish>> GetBirthdayWishesByEmployee(int birthdayEmployeeId, int companyId, int wishYear, int requestingEmployeeId);
        Task<vmToggleBirthdayWishLikeResult> ToggleBirthdayWishLike(vmToggleBirthdayWishLike model);
        Task<List<vmBirthdayWishComment>> GetBirthdayWishComments(int birthdayWishId);
        Task<vmBirthdayWishActionResult> CreateBirthdayWishComment(vmCreateBirthdayWishComment model);
        Task<VMCommonResult> DeleteBirthdayWishComment(int birthdayWishCommentId, int employeeId);
    }
}
