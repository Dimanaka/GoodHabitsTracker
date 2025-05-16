using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.Maui.Controls;
using System;

namespace HabitTracker
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private async void OnAddHabitClicked(object sender, EventArgs e)
        {
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
        }
    }
}