using StaffSchedulingSystem.Models;

namespace StaffSchedulingSystem.Repos.Interfaces
{
    public interface IRoleRepository
    {
        Role GetRoleByName(string name);
        Role getStaffRole(string name= "Staff");
    }
}
