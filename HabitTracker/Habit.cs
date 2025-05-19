using SQLite;

namespace HabitTracker.Models
{
    public class Habit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}