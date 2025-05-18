using HabitTracker.Models;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HabitTracker.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "habits.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Habit>().Wait();
        }
        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Habit>().Wait();
        }
        public Task<int> AddHabitAsync(Habit habit)
        {
            if (string.IsNullOrWhiteSpace(habit.Name))
            {
                throw new ArgumentException("Habit name cannot be empty", nameof(habit.Name));
            }
            if (habit.Name.Length > 50)
            {
                throw new ArgumentException("Habit name cannot exceed 50 characters", nameof(habit.Name));
            }
          
            habit.CreatedDate = DateTime.Now;
            return _database.InsertAsync(habit); 
        }

        public Task<List<Habit>> GetHabitsAsync()
        {
            return _database.Table<Habit>().ToListAsync();
        }

        public Task<int> DeleteHabitAsync(Habit habit)
        {
            return _database.DeleteAsync(habit);
        }
        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }

        public async Task CloseConnectionAsync()
        {
            await _database.CloseAsync();
        }
    }
}