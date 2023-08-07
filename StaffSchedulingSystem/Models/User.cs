
namespace StaffSchedulingSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int? ScheduleId { get; set; }

        public List<Schedule> Schedules { get; set; }

        public Role Role { get; set; }

    }
}
