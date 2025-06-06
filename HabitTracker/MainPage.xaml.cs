﻿using HabitTracker.Models;
using HabitTracker.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HabitTracker
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Habit> _habits;
        public ObservableCollection<Habit> Habits
        {
            get => _habits;
            set
            {
                _habits = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            BindingContext = this;
            LoadHabits();
        }

        // Load habits from database for display
        private async void LoadHabits()
        {
            var habits = await _databaseService.GetHabitsAsync();
            Habits = new ObservableCollection<Habit>(habits);
        }

        private async void OnAddHabitClicked(object sender, EventArgs e)
        {
            // Sentry error tracking testing
            SentrySdk.CaptureMessage("Habit added successfully");

            if (string.IsNullOrWhiteSpace(HabitNameEntry.Text?.Trim()))
            {
                await DisplayAlert("Error", "Please enter a habit name", "OK");
                return;
            }

            if (FrequencyPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a frequency", "OK");
                return;
            }

            if (HabitNameEntry.Text.Trim().Length > 50)
            {
                await DisplayAlert("Error", "Habit name cannot exceed 50 characters", "OK");
                return;
            }

            var habit = new Habit
            {
                Name = HabitNameEntry.Text.Trim(),
                Frequency = FrequencyPicker.SelectedItem.ToString(),
                CreatedDate = DateTime.Now
            };

            await _databaseService.AddHabitAsync(habit);
            await DisplayAlert("Success", "Habit added successfully", "OK");
            HabitNameEntry.Text = string.Empty;
            FrequencyPicker.SelectedIndex = -1;
            LoadHabits();
        }
        private async void OnDeleteHabitClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Habit habit)
            {
                var confirm = await DisplayAlert("Confirm", $"Delete habit '{habit.Name}'?", "Yes", "No");
                if (confirm)
                {
                    await _databaseService.DeleteHabitAsync(habit);
                    LoadHabits();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}