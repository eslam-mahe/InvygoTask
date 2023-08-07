using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StaffSchedulingSystem.Repos
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ScheduleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Schedule newSchedule)
        {
            await _dbContext.Schedules.AddAsync(newSchedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int scheduleId)
        {
            await _dbContext.Schedules.Where(s => s.Id == scheduleId).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Schedule>> GetSchedules()
        {
            return await _dbContext.Schedules.Include(X=>X.User).ToListAsync();
        }

        public async Task<bool> Update(int scheduleId, Schedule newSchedule)
        {
            Schedule current  = await _dbContext.Schedules.FirstOrDefaultAsync(X => X.Id == scheduleId);
            if (current==null) return false;

            current.ShiftLengthHours = newSchedule.ShiftLengthHours >0 ? newSchedule.ShiftLengthHours : current.ShiftLengthHours;
            current.WorkDate = newSchedule.WorkDate;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task<List<Schedule>> ViewSchedule(string userName, DateTime startTime, DateTime endTime)
        {
           return _dbContext.Schedules.Where(s => s.User.UserName == userName && s.WorkDate >= startTime && s.WorkDate <= endTime).Include(x => x.User).ToListAsync();
        }

        public Task<List<Schedule>> ViewTeamMateSchedule(string name, DateTime startTime, DateTime endTime)
        {
            return _dbContext.Schedules.Where(s => (s.User.Name.Equals(name)|| s.User.Name.Contains(name) ) && s.WorkDate >= startTime && s.WorkDate <= endTime).Include(x=>x.User).ToListAsync();
        }

    }
}