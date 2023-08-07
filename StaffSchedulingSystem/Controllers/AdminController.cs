using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StaffSchedulingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ScheduleService _scheduleService;
        public AdminController(UserService userService, ScheduleService scheduleService)
        {
            _userService = userService;
            _scheduleService = scheduleService;
        }

        [HttpGet("users")]

        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsers());
        }
        [HttpPut("MakeAdminUser")]
        public async Task<IActionResult> UpdateUserRoletoAdmin([FromBody]string userName)
        {
            if (String.IsNullOrEmpty( userName )) {
                return BadRequest("UserName is not valid");
            }
            bool result= await _userService.EditUserRoleToAdmin(userName);
            if (!result) {
                return BadRequest("Something went wrong please check your input");
            }
            return Ok("Role updated sucessfully");
        }
        [HttpPut("MakeStaffUser")]
        public async Task<IActionResult> UpdateUserRoletoStaff([FromBody] string userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                return BadRequest("UserName is not valid");
            }
            bool result = await _userService.EditUserRoleToStaff(userName);
            if (!result)
            {
                return BadRequest("Something went wrong please check your input");
            }
            return Ok("Role updated sucessfully");
        }
        [HttpPut("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserinfo([FromBody] string currentUserName,[FromBody] User user)
        {
            if (user== null)
            {
                return BadRequest("User is not valid");
            }
            bool result = await _userService.EditUserInfo(currentUserName,user);
            if (!result)
            {
                return BadRequest("Something went wrong please check your input");
            }
            return Ok("User updated sucessfully");
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string currentUserName) 
        {
            await _userService.DeleteUser(currentUserName);
            return Ok();
        }
        [HttpGet("Schedules")]
        public async Task<IActionResult> GetSchedules()
        {
            return Ok(await _scheduleService.GetSchedules());
        }
        [HttpGet("UsersHoursPerPeriod")]
        public async Task<IActionResult> GetUsersHoursPerPeriod([FromBody] ScheduleRequest periodOfTime )
        {
            return Ok(await _scheduleService.GetUsersHoursPerPeriod(periodOfTime.startDate, periodOfTime.endDate));
        }
        [HttpPost("CreateSchedule")]
        public async Task CreateSchedules([FromBody] Schedule newSchedule) 
        {
            await _scheduleService.Create(newSchedule);
        }
        [HttpPut("UpdateSchedule")]
        public async Task UpdateSchedule([FromBody] Schedule newSchedule)
        {
            await _scheduleService.Update(newSchedule.Id, newSchedule);
        }
        [HttpDelete("DeleteSchedule")]
        public async Task DeleteSchedule([FromRoute] int scheduleId)
        {
            await _scheduleService.Delete(scheduleId);
        }
    }
}
