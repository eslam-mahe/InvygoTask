using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;

namespace StaffSchedulingSystem.Repos
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role GetRoleByName(string name)
        {
            return _dbContext.Roles.Where(r => r.Name == name).FirstOrDefault();
        }

        public Role getStaffRole(string name= "Staff")
        {
            return _dbContext.Roles.Where(r => r.Name == name).FirstOrDefault();
        }
    }
}
