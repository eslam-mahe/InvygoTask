using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;
using StaffSchedulingSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvygoTaskTest.Services
{
    public class ScheduleServiceTests
    {
        [Fact]
        public async Task Create_Should_Call_Repository_Create_Method()
        {
            // Arrange
            var mockScheduleRepository = new Mock<IScheduleRepository>();
            ScheduleService scheduleService = new ScheduleService(mockScheduleRepository.Object);
            Schedule newSchedule = new Schedule();

            // Act
            await scheduleService.Create(newSchedule);

            // Assert
            mockScheduleRepository.Verify(repo => repo.Create(newSchedule), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Call_Repository_Delete_Method()
        {
            // Arrange
            var mockScheduleRepository = new Mock<IScheduleRepository>();
            ScheduleService scheduleService = new ScheduleService(mockScheduleRepository.Object);
            int scheduleId = 1;

            // Act
            await scheduleService.Delete(scheduleId);

            // Assert
            mockScheduleRepository.Verify(repo => repo.Delete(scheduleId), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Call_Repository_Update_Method_And_Return_True_On_Success()
        {
            // Arrange
            var mockScheduleRepository = new Mock<IScheduleRepository>();
            ScheduleService scheduleService = new ScheduleService(mockScheduleRepository.Object);
            int scheduleId = 1;
            Schedule newSchedule = new Schedule();

            mockScheduleRepository.Setup(repo => repo.Update(scheduleId, newSchedule)).ReturnsAsync(true);

            // Act
            bool result = await scheduleService.Update(scheduleId, newSchedule);

            // Assert
            Assert.True(result);
            mockScheduleRepository.Verify(repo => repo.Update(scheduleId, newSchedule), Times.Once);
        }

        // Add test cases for the other methods (ViewSchedule, ViewTeamMateSchedule, GetSchedules) in a similar manner.

        [Fact]
        public async Task GetUsersHoursPerPeriod_Should_Calculate_Users_Hours_Correctly()
        {
            // Arrange
            var mockScheduleRepository = new Mock<IScheduleRepository>();
            ScheduleService scheduleService = new ScheduleService(mockScheduleRepository.Object);

            // Set up a list of schedules for testing
            List<Schedule> schedules = new List<Schedule>
            {
                new Schedule { Id = 1, UserId = 1, ShiftLengthHours = 8, WorkDate = new DateTime(2023, 8, 1) },
                new Schedule { Id = 2, UserId = 1, ShiftLengthHours = 6, WorkDate = new DateTime(2023, 8, 2) },
                new Schedule { Id = 3, UserId = 2, ShiftLengthHours = 7, WorkDate = new DateTime(2023, 8, 1) }
            };

            mockScheduleRepository.Setup(repo => repo.GetSchedules()).ReturnsAsync(schedules);

            // Act
            List<UsersHours> result = await scheduleService.GetUsersHoursPerPeriod(new DateTime(2023, 8, 1), new DateTime(2023, 8, 2));

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].userId);
            Assert.Equal(14, result[0].hoursCount); // 8 + 6 = 14
            Assert.Equal(new DateTime(2023, 8, 1), result[0].startTime);
            Assert.Equal(new DateTime(2023, 8, 2), result[0].endTime);

            Assert.Equal(2, result[1].userId);
            Assert.Equal(7, result[1].hoursCount); // 7
            Assert.Equal(new DateTime(2023, 8, 1), result[1].startTime);
            Assert.Equal(new DateTime(2023, 8, 2), result[1].endTime);
        }
    }
}
