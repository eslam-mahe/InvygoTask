using invygo_Task.Models;
using invygo_Task.Repos.Interfaces;
using invygo_Task.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvygoTaskTest.Services
{
    public class RoleServiceTests
    {
        [Fact]
        public void GetStaffRole_Should_Return_StaffRole()
        {
            // Arrange
            Role expectedRole = new Role { Id = 1, Name = "Staff" };

            // Create a mock IRoleRepository and set up the getStaffRole method to return the expected role
            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(repo => repo.GetRoleByName("Staff")).Returns(expectedRole);

            // Create the RoleService instance and pass the mock repository
            RoleService roleService = new RoleService(mockRoleRepository.Object);

            // Act
            Role actualRole = roleService.getStaffRole();

            // Assert
            Assert.Equal(expectedRole.Id, actualRole.Id);
            Assert.Equal(expectedRole.Name, actualRole.Name);
        }

        [Fact]
        public void GetStaffRole_Should_Return_EmptyRole_On_Exception()
        {
            // Arrange
            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(repo => repo.GetRoleByName("Staff")).Throws(new Exception("Error fetching role."));

            RoleService roleService = new RoleService(mockRoleRepository.Object);

            // Act
            Role actualRole = roleService.getStaffRole();

            // Assert
            Assert.Equal(default(int), actualRole.Id); // Assuming default value for RoleId is 0
            Assert.Null(actualRole.Name);
        }
    }
}
