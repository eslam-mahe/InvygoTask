using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StaffSchedulingSystem.Services
{
    public class ScheduleService
    {
        private IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository ) {
        
            _scheduleRepository = scheduleRepository;
        }


        public async Task Create(Schedule newSchedule)
        {
           await _scheduleRepository.Create(newSchedule);
        }

        public async Task Delete(int scheduleId)
        {
            await _scheduleRepository.Delete(scheduleId);
        }

        public async Task<bool> Update(int scheduleId, Schedule newSchedule)
        {
           return await _scheduleRepository.Update(scheduleId, newSchedule);
        }

        public async Task<List<Schedule>> ViewSchedule(string userName, DateTime startTime, DateTime endTime)
        {
            return await _scheduleRepository.ViewSchedule(userName, startTime, endTime);
        }

        public async Task<List<Schedule>> ViewTeamMateSchedule(string name, DateTime startTime, DateTime endTime)
        {
            return await _scheduleRepository.ViewTeamMateSchedule(name, startTime, endTime);
        }
        public async Task<List<Schedule>> GetSchedules()
        {
            return await _scheduleRepository.GetSchedules();
        }

        public async Task<List<UsersHours>> GetUsersHoursPerPeriod(DateTime startDate, DateTime endDate)
        {
            List<Schedule> schedules = await _scheduleRepository.GetSchedules();
            List<UsersHours> userHours = schedules
                .Where(schedule => schedule.WorkDate >= startDate && schedule.WorkDate <= endDate)
                .GroupBy(schedule => schedule.UserId)
                .Select(group => new UsersHours
                {
                    userId = group.Key,
                    hoursCount = group.Sum(schedule => schedule.ShiftLengthHours),
                    userName = group.FirstOrDefault()?.User?.Name,
                    startTime = startDate,
                    endTime = endDate
                })
                .ToList();
            return userHours;
        }
    }
}
