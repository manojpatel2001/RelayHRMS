using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class CityRepository : Repository<City> ,ICityRepository
    {
        private readonly HRMSDbContext _db;

        public CityRepository(HRMSDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<bool> UpdateCity(City city)
        {
            var existingRecord = await _db.City.SingleOrDefaultAsync(asd => asd.CityID == city.CityID);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.CityName = city.CityName;
            existingRecord.StateId = city.StateId;
            existingRecord.CityCategoryId = city.CityCategoryId;
            existingRecord.Remarks = city.Remarks;
            existingRecord.UpdatedBy = city.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        public async Task<City> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var city = await _db.City.FirstOrDefaultAsync(asd => asd.CityID == DeleteRecord.Id);
            if (city == null)
            {
                return city;
            }
            else
            {
                city.IsEnabled = false;
                city.IsDeleted = true;
                city.DeletedDate = DateTime.UtcNow;
                city.DeletedBy = DeleteRecord.DeletedBy;
                return city;
            }
        }
    }
}
