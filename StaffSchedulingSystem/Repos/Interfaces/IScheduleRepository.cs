using StaffSchedulingSystem.Models;

namespace StaffSchedulingSystem.Repos.Interfaces
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> ViewSchedule(string userName, DateTime startTime,DateTime endTime);
        Task<List<Schedule>> ViewTeamMateSchedule(string name, DateTime startTime, DateTime endTime);
        Task<List<Schedule>> GetSchedules();
        Task Create(Schedule newSchedule);
        Task<bool> Update(int scheduleId,Schedule newSchedule);
        Task Delete(int scheduleId);
    }
}