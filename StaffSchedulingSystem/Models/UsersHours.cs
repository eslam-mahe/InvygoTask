namespace StaffSchedulingSystem.Models
{
    public class UsersHours
    {
        public string userName { get; set; }
        public int userId { get; set; }

        public int hoursCount { get; set; }
        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }
    }
}
