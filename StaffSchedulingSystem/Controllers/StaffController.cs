using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace StaffSchedulingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]

    public class StaffController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ScheduleService _scheduleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StaffController(UserService userService, ScheduleService scheduleService , IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _scheduleService = scheduleService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("users")]

        public async Task<List<User>> Get()
        {
            return await _userService.GetUsers();
        }

        [HttpGet("MySchedule")]
        public async Task<IActionResult> GetMySchedules([FromBody] ScheduleRequest scheduleRequest) 
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext!=null && httpContext.User !=null && httpContext.User.Identity!=null && httpContext.User.Identity.IsAuthenticated)
            {
                // Get the username claim from the token
                var username = httpContext?.User?.Identity?.Name;


                return Ok(_scheduleService.ViewSchedule(username, scheduleRequest.startDate, scheduleRequest.endDate));
            }
            else
                return Unauthorized();
        }
        [HttpGet("TeamMateSchedule")]
        public async Task<IActionResult> GetTeamMateSchedules([FromBody] TeamMateScheduleRequest teamMateScheduleRequest)
        {
                return Ok(_scheduleService.ViewTeamMateSchedule(teamMateScheduleRequest.teamMateName, teamMateScheduleRequest.scheduleRequest.startDate, teamMateScheduleRequest.scheduleRequest.endDate));
        }
    }
}
