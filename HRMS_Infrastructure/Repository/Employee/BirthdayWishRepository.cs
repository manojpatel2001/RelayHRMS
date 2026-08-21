using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    internal class BirthdayWishRepository : IBirthdayWishRepository
    {
        private readonly HRMSDbContext _db;

        public BirthdayWishRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateBirthdayWish(vmCreateBirthdayWish model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageBirthdayWish
                    @Action = {"CREATE"},
                    @BirthdayEmployeeId = {model.BirthdayEmployeeId},
                    @WishedByEmployeeId = {model.WishedByEmployeeId},
                    @CompanyId = {model.CompanyId},
                    @Message = {model.Message},
                    @WishYear = {System.DateTime.UtcNow.Year}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteBirthdayWish(int birthdayWishId, int wishedByEmployeeId)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageBirthdayWish
                    @Action = {"DELETE"},
                    @BirthdayWishId = {birthdayWishId},
                    @WishedByEmployeeId = {wishedByEmployeeId}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<vmBirthdayWish>> GetBirthdayWishesByEmployee(int birthdayEmployeeId, int companyId, int wishYear, int requestingEmployeeId)
        {
            try
            {
                return await _db.Set<vmBirthdayWish>().FromSqlInterpolated($@"
                EXEC GetBirthdayWishesByEmployee
                    @BirthdayEmployeeId = {birthdayEmployeeId},
                    @CompanyId = {companyId},
                    @WishYear = {wishYear},
                    @RequestingEmployeeId = {requestingEmployeeId}
                ").ToListAsync();
            }
            catch
            {
                return new List<vmBirthdayWish>();
            }
        }

        public async Task<vmToggleBirthdayWishLikeResult> ToggleBirthdayWishLike(vmToggleBirthdayWishLike model)
        {
            try
            {
                var result = await _db.Set<vmToggleBirthdayWishLikeResult>().FromSqlInterpolated($@"
                EXEC ManageBirthdayWishLike
                    @BirthdayWishId = {model.BirthdayWishId},
                    @EmployeeId = {model.EmployeeId}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new vmToggleBirthdayWishLikeResult { IsLikedNow = false };
            }
            catch
            {
                return new vmToggleBirthdayWishLikeResult { IsLikedNow = false };
            }
        }

        public async Task<List<vmBirthdayWishComment>> GetBirthdayWishComments(int birthdayWishId)
        {
            try
            {
                return await _db.Set<vmBirthdayWishComment>().FromSqlInterpolated($"EXEC GetBirthdayWishComments @BirthdayWishId = {birthdayWishId}").ToListAsync();
            }
            catch
            {
                return new List<vmBirthdayWishComment>();
            }
        }

        public async Task<vmBirthdayWishActionResult> CreateBirthdayWishComment(vmCreateBirthdayWishComment model)
        {
            try
            {
                var result = await _db.Set<vmBirthdayWishActionResult>().FromSqlInterpolated($@"
                EXEC ManageBirthdayWishComment
                    @Action = {"CREATE"},
                    @BirthdayWishId = {model.BirthdayWishId},
                    @EmployeeId = {model.EmployeeId},
                    @CommentText = {model.CommentText}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new vmBirthdayWishActionResult { Id = 0 };
            }
            catch
            {
                return new vmBirthdayWishActionResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteBirthdayWishComment(int birthdayWishCommentId, int employeeId)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageBirthdayWishComment
                    @Action = {"DELETE"},
                    @BirthdayWishCommentId = {birthdayWishCommentId},
                    @EmployeeId = {employeeId}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }
    }
}
