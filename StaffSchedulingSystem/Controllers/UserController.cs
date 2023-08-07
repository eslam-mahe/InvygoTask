using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaffSchedulingSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;

        public UserController( UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Replace this with actual password hashing and validation logic
            if (! await _userService.isUserExist(request.Username, request.Password))
            {
                return Unauthorized();
            }

            var token = await _userService.GenerateJwtToken(request.Username, request.Password);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            
            await _userService.Register(request);
            var token = _userService.GenerateJwtToken(request.Username, request.Password);

            return Ok(new { Token = token });
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userService.GetUsers();
        }
    }
}
