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

        public Task<int> AddHabitAsync(Habit habit)
        {
            return _database.InsertAsync(habit);
        }

        public Task<List<Habit>> GetHabitsAsync()
        {
            return _database.Table<Habit>().ToListAsync();
        }
    }
}