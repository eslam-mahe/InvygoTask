using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StaffSchedulingSystem.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllAsync() => await _dbContext.Users.ToListAsync();

        public async Task<bool> IsExists(string userName, string password)
        {
            return  _dbContext.Users.Where(x => x.UserName == userName && x.Password == password).Any();
        }
        public async Task<User> GetAsync(string userName, string password) => await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
        public async Task Create(User user) 
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<bool> UpdateRole(string userName, string roleName)
        {
            User current = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            Role newRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
            if (current == null|| newRole == null) {
                return false;
            }
            current.RoleId = newRole.Id;
            _dbContext.SaveChanges();

            return true;
        }

        public async Task Delete(string userName)
        {
            await _dbContext.Users.Where(x =>x.UserName == userName).ExecuteDeleteAsync();
            _dbContext.SaveChanges();
        }

        public async Task<bool> UpdateInfo(string userName, User newUser)
        {
            User current = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (current == null)
            {
                return false;
            }
            current.Name = String.IsNullOrEmpty( newUser.Name) ? current.Name : newUser.Name;
            current.UserName = String.IsNullOrEmpty(newUser.UserName) ? current.UserName : newUser.UserName;
            current.Password = String.IsNullOrEmpty(newUser.Password) ? current.Password : newUser.Password;

            _dbContext.SaveChanges();

            return true;
        }
    }
}
