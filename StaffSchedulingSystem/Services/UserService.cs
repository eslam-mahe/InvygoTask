using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StaffSchedulingSystem.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly RoleService _roleService;

        public UserService(IUserRepository userRepository, IConfiguration configuration ,RoleService roleService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _roleService = roleService;
        }
        public async Task<List<User>> GetUsers()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<User>();
            }
        }
        public Task<bool> isUserExist(string userName, string password) {

            return  _userRepository.IsExists(userName,password);
        }

        public async Task<string> GenerateJwtToken(string userName, string password)
        {
            User currentUser =await _userRepository.GetAsync(userName, password);


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("jwtKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, currentUser.UserName),
                new Claim(ClaimTypes.Role, currentUser.Role.Name)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task Register(RegisterRequest request)
        {
            Role defaultStaff = _roleService.getStaffRole();
            User newUser = new User {
                Name = request.Name,
                UserName = request.Username,
                Password = request.Password,
                Role = defaultStaff,
                RoleId= defaultStaff.Id
            };
           await _userRepository.Create(newUser);
        }
        public async Task<bool> EditUserRoleToAdmin(string userName)
        {
            return await _userRepository.UpdateRole(userName, "Admin");
        }
        public async Task<bool> EditUserRoleToStaff(string userName)
        {
            return await _userRepository.UpdateRole(userName, "Staff");
        }
        public async Task<bool> EditUserInfo(string userName,User newUser)
        {
            return await _userRepository.UpdateInfo(userName, newUser);
        }
        public async Task DeleteUser(string userName)
        {
             await _userRepository.Delete(userName);
        }
    }
}
