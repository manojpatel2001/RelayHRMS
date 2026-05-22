using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class LeaveMasterRepository:Repository<LeaveMaster>,ILeaveMasterRepository
    {

        private HRMSDbContext _db;

        public LeaveMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<LeaveMaster>> GetLeaveMaster(int CompId)
        {
            try
            {
                var result = await _db.Set<LeaveMaster>()
                    .FromSqlInterpolated($"EXEC SP_GetAllLeaveMaster @CompId = {CompId}")
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SP_GetAllLeaveMaster Error: " + ex.Message);
                return new List<LeaveMaster>();
            }
        }

        public async Task<List<LeaveTypeViewModel>> GetLeaveTypesForEmployee(int CompId, int Empid)
        {
            try
            {
                var result = await _db.Set<LeaveTypeViewModel>()
                    .FromSqlInterpolated($"EXEC GetLeaveTypesForEmployee @CompId = {CompId} ,@Empid ={Empid}")
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveTypesForEmployee Error: " + ex.Message);
                return new List<LeaveTypeViewModel>();
            }
        }
    }
}
