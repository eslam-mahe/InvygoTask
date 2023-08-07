using StaffSchedulingSystem.Models;
using StaffSchedulingSystem.Repos.Interfaces;

namespace StaffSchedulingSystem.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository  roleRepository) {
            _roleRepository = roleRepository;
        }
        public Role getStaffRole() {
            try
            {
                return _roleRepository.GetRoleByName("Staff");
            }
            catch (Exception ex){
                
            Console.WriteLine(ex.Message );
                return new Role();
            }
        }
    }
}
