using HRMS_Core.DbContext;
using HRMS_Core.Report;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.Report;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Report
{
    public class AddEventRepository : Repository<AddEvent>, IAddeventRepository
    {

        private readonly HRMSDbContext _db;
        public AddEventRepository(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }
        public async Task<SP_Response> CreateEvent(AddEvent model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                        EXEC sp_AddEvent_CRUD
                            @Operation = {"INSERT"},
                            @EventName = {model.EventName},
                            @Date = {model.Date},
                            @EventType = {model.EventType},
                            @Repeat = {model.Repeat},
                            @IsMyevent = {model.IsMyevent},
                            @IsShowall = {model.IsShowall},
                            @IsDeleted = {model.IsDeleted},
                            @IsEnabled = {model.IsEnabled},
                            @CreatedBy = {model.CreatedBy}
                        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<VMCommonResult> DeleteEvent(DeleteRecordVModel deleteRecordVM)
        {

            try
            {
                string ids = string.Join(",", deleteRecordVM.Id); // Convert List<int> to "1,2,3"

                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
            EXEC DeleteEvent                    
                @Ids = {ids},
                @DeletedBy = {deleteRecordVM.DeletedBy}
        ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }


           
        public async Task<List<EventModelVM>> GetAllEvent(DateTime TargetDate)
        {
            try
            {
                var result = await _db.Set<EventModelVM>().FromSqlInterpolated($"EXEC GetAllEvents @TargetDate={TargetDate}").ToListAsync();
                return result;
            }
            catch
            {
                return new List<EventModelVM>();
            }

        }

        public async Task<AddEvent?> GetEventById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<AddEvent>().FromSqlInterpolated($@"
                        EXEC sp_AddEvent_CRUD
                            @Operation = {"GET"},
                            @EventId = {filter.Id}
                        ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        

        public async Task<SP_Response> UpdateEvent(AddEvent model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                            EXEC sp_AddEvent_CRUD
                                @Operation = {"UPDATE"},
                                @EventId = {model.EventId},
                                @EventName = {model.EventName},
                                @Date = {model.Date},
                                @EventType = {model.EventType},
                                @Repeat = {model.Repeat},
                                @IsMyevent = {model.IsMyevent},
                                @IsShowall = {model.IsShowall},
                                @IsDeleted = {model.IsDeleted},
                                @IsEnabled = {model.IsEnabled},
                                @IsBlocked = {model.IsBlocked},
                                @UpdatedBy = {model.UpdatedBy}
                            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }
    }
}
