namespace StaffSchedulingSystem.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime WorkDate { get; set; }
        public int UserId { get; set; }
        public int ShiftLengthHours { get; set; }
        // Other properties as needed

        // Navigation property
        public User User { get; set; }
    }
}
