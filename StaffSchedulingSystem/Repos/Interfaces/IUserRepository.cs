using StaffSchedulingSystem.Models;

namespace StaffSchedulingSystem.Repos.Interfaces
{
    public interface IUserRepository
    {
         Task<List<User>> GetAllAsync();
         Task<bool> IsExists(string userName, string password);
         Task<User> GetAsync(string userName, string password);
         Task Create(User user);
         Task<bool> UpdateInfo(string userName, User newUser);
         Task<bool> UpdateRole(string userName, string roleName);
         Task Delete(string userName);
    }
}
