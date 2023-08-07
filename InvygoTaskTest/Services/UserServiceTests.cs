using invygo_Task.Models;
using invygo_Task.Repos.Interfaces;
using invygo_Task.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InvygoTaskTest.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> mockUserRepository;
        private IConfiguration mockConfiguration;
        private RoleService roleService;
        private Mock<IRoleRepository> mockRoleRepository; // Mock the IRoleRepository

        private UserService userService;

        public UserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockConfiguration = GetMockConfiguration();
            mockRoleRepository = new Mock<IRoleRepository>(); // Initialize the mock RoleRepository

            roleService = new RoleService(mockRoleRepository.Object); // Inject the mock RoleRepository

            userService = new UserService(mockUserRepository.Object, mockConfiguration, roleService);
        }
        private IConfiguration GetMockConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "jwtKey", "your_secret_key_here" } // Replace with your secret key for testing
                })
                .Build();

            return configuration;
        }
        [Fact]
        public async Task GetUsers_Should_Return_All_Users()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { UserName = "user1", Role = new Role { Name = "Staff" } },
                new User { UserName = "user2", Role = new Role { Name = "Admin" } }
            };

            mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedUsers);

            // Act
            var result = await userService.GetUsers();

            // Assert
            Assert.Equal(expectedUsers, result);
        }

        [Fact]
        public async Task GenerateJwtToken_Should_Return_Valid_Token()
        {
            // Arrange
            var currentUser = new User { UserName = "user1", Role = new Role { Name = "Staff" } };

            mockUserRepository.Setup(repo => repo.GetAsync("user1", "password123")).ReturnsAsync(currentUser);

            // Act
            var result = await userService.GenerateJwtToken("user1", "password123");

            // Assert
            Assert.NotNull(result);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(result);
            Assert.Equal("user1", token.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value);
            Assert.Equal("Staff", token.Claims.FirstOrDefault(c => c.Type == "role")?.Value);
        }
        [Fact]
        public async Task IsUserExist_Should_Return_True_If_User_Exists()
        {
            // Arrange
            string userName = "user1";
            string password = "password123";

            mockUserRepository.Setup(repo => repo.IsExists(userName, password)).ReturnsAsync(true);

            // Act
            var result = await userService.isUserExist(userName, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsUserExist_Should_Return_False_If_User_Does_Not_Exist()
        {
            // Arrange
            string userName = "user1";
            string password = "password123";

            mockUserRepository.Setup(repo => repo.IsExists(userName, password)).ReturnsAsync(false);

            // Act
            var result = await userService.isUserExist(userName, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Register_Should_Create_User_With_Default_Staff_Role()
        {
            // Arrange
            var defaultStaff = new Role { Id = 1, Name = "Staff" };
            var request = new RegisterRequest { Name = "John Doe", Username = "john", Password = "password123" };

            mockRoleRepository.Setup(repo => repo.GetRoleByName("Staff")).Returns(defaultStaff); // Mock GetRoleByName

            User createdUser = null;
            mockUserRepository.Setup(repo => repo.Create(It.IsAny<User>())).Callback<User>(user => createdUser = user);

            // Act
            await userService.Register(request);

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(request.Name, createdUser.Name);
            Assert.Equal(request.Username, createdUser.UserName);
            Assert.Equal(request.Password, createdUser.Password);
            Assert.Equal(defaultStaff, createdUser.Role);
            Assert.Equal(defaultStaff.Id, createdUser.RoleId);
        }

        [Fact]
        public async Task EditUserRoleToAdmin_Should_Update_User_Role_To_Admin()
        {
            // Arrange
            string userName = "user1";
            mockUserRepository.Setup(repo => repo.UpdateRole(userName, "Admin")).ReturnsAsync(true);

            // Act
            var result = await userService.EditUserRoleToAdmin(userName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditUserRoleToStaff_Should_Update_User_Role_To_Staff()
        {
            // Arrange
            string userName = "user1";
            mockUserRepository.Setup(repo => repo.UpdateRole(userName, "Staff")).ReturnsAsync(true);

            // Act
            var result = await userService.EditUserRoleToStaff(userName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditUserInfo_Should_Update_User_Info()
        {
            // Arrange
            string userName = "user1";
            User newUser = new User { Name = "New Name", UserName = "newusername", Password = "newpassword" };
            mockUserRepository.Setup(repo => repo.UpdateInfo(userName, newUser)).ReturnsAsync(true);

            // Act
            var result = await userService.EditUserInfo(userName, newUser);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUser_Should_Delete_User()
        {
            // Arrange
            string userName = "user1";
            mockUserRepository.Setup(repo => repo.Delete(userName)).Returns(Task.CompletedTask);

            // Act & Assert (no need to assert for void methods)
            await userService.DeleteUser(userName);
        }
    }
}
