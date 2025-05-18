using HabitTracker.Models;
using HabitTracker.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace HabitsTracker.Test
{
    public class DatabaseServiceTests : IDisposable
    {
        private readonly DatabaseService _databaseService; 
        private readonly string _dbPath;

        public DatabaseServiceTests()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), "test_habits.db3");
            _databaseService = new DatabaseService(_dbPath);
            _databaseService.GetConnection().ExecuteAsync("DELETE FROM Habit").Wait();
        }

        public void Dispose()
        {
            // Закриття з’єднання після кожного тесту
            _databaseService.CloseConnectionAsync().GetAwaiter().GetResult();
            if (File.Exists(_dbPath))
            {
                try
                {
                    File.Delete(_dbPath);
                }
                catch (IOException)
                {
                    // Ігноруємо помилку, якщо файл заблоковано
                }
            }
        }

        [Fact]
        public async Task AddHabitAsync_ValidHabit_ReturnsSuccess()
        {
            var habit = new Habit { Name = "Test Habit", Frequency = "Daily", CreatedDate = DateTime.Now };
            var result = await _databaseService.AddHabitAsync(habit);
            Assert.Equal(1, result);
            var habits = await _databaseService.GetHabitsAsync();
            Assert.Single(habits);
            Assert.Equal("Test Habit", habits[0].Name);
        }

        [Fact]
        public async Task AddHabitAsync_EmptyName_ThrowsArgumentException()
        {
            var habit = new Habit { Name = "", Frequency = "Weekly", CreatedDate = DateTime.Now };
            await Assert.ThrowsAsync<ArgumentException>(() => _databaseService.AddHabitAsync(habit));
        }

        [Fact]
        public async Task GetHabitsAsync_EmptyDatabase_ReturnsEmptyList()
        {
            var habits = await _databaseService.GetHabitsAsync();
            Assert.Empty(habits);
        }

        [Fact]
        public async Task GetHabitsAsync_WithHabits_ReturnsCorrectList()
        {
            var habit1 = new Habit { Name = "Habit 1", Frequency = "Daily", CreatedDate = DateTime.Now };
            var habit2 = new Habit { Name = "Habit 2", Frequency = "Weekly", CreatedDate = DateTime.Now };
            await _databaseService.AddHabitAsync(habit1);
            await _databaseService.AddHabitAsync(habit2);
            var habits = await _databaseService.GetHabitsAsync();
            Assert.Equal(2, habits.Count);
            Assert.Contains(habits, h => h.Name == "Habit 1");
            Assert.Contains(habits, h => h.Name == "Habit 2");
        }

        [Fact]
        public async Task DeleteHabitAsync_ExistingHabit_RemovesHabit()
        {
            var habit = new Habit { Name = "Test Habit", Frequency = "Daily", CreatedDate = DateTime.Now };
            await _databaseService.AddHabitAsync(habit);
            var habits = await _databaseService.GetHabitsAsync();
            var habitToDelete = habits[0];
            var result = await _databaseService.DeleteHabitAsync(habitToDelete);
            Assert.Equal(1, result);
            habits = await _databaseService.GetHabitsAsync();
            Assert.Empty(habits);
        }

        [Fact]
        public async Task DeleteHabitAsync_NonExistingHabit_ReturnsZero()
        {
            var habit = new Habit { Id = 999, Name = "Non-existing", Frequency = "Daily", CreatedDate = DateTime.Now };
            var result = await _databaseService.DeleteHabitAsync(habit);
            Assert.Equal(0, result);
        }
    }
}