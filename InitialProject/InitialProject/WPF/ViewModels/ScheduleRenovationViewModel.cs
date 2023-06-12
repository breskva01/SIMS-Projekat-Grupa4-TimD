using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class ScheduleRenovationViewModel : ViewModelBase, IDataErrorInfo
    {
        private Accommodation _selectedAccommodation;
        private DateTime _start = DateTime.Now;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _end = DateTime.Now;
        public DateTime End
        {
            get => _end;
            set
            {
                if (value != _end)
                {
                    _end = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _duration = "1";
        public string Duration
        {
            get => _duration;
            set 
            {
                if (value != _duration) 
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private AccommodationReservationService _acommodationReservationService;
        private AccommodationRenovationService _accommodationRenovationService;
        private AccommodationService _accommodationService;
        public ObservableCollection<TimeSlot> AvailableDates { get; set; }
        public TimeSlot SelectedTimeSlot { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand ScheduleRenovationCommand { get; }
        public ScheduleRenovationViewModel(Accommodation selectedAccommodation) 
        {
            _selectedAccommodation = selectedAccommodation;
            _acommodationReservationService = new AccommodationReservationService();
            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationService = new AccommodationService();
            AvailableDates = new ObservableCollection<TimeSlot>();
            SearchCommand = new ExecuteMethodCommand(Search);
            ScheduleRenovationCommand = new ExecuteMethodCommand(ScheduleRenovation);
        }
        private void Search()
        {
            if (Duration == "0" || Duration == null)
            {
                MessageBox.Show("Choose duration!");
            }
            else
            {
                var availableDates = _acommodationReservationService.GetAvailableDates(Start, End, int.Parse(Duration) - 1, _selectedAccommodation.Id);
                AvailableDates.Clear();
                foreach (var date in availableDates)
                {
                    AvailableDates.Add(date);
                }
            }
        }
        private void ScheduleRenovation()
        {
            if (Description == null || Description == "" || SelectedTimeSlot == null)
            {
                MessageBox.Show("You have to fill out the form!");
            }
            else
            {
                _accommodationRenovationService.ScheduleRenovation(_selectedAccommodation.Id, SelectedTimeSlot.Start, SelectedTimeSlot.End, Description);
                _accommodationService.UpdateRenovationStatus(_selectedAccommodation.Id);
                Start = DateTime.Now;
                End = DateTime.Now;
                Duration = "";
                Description = "";
                AvailableDates.Clear();
            }
        }
        private Regex _numberRegex = new Regex("^[1-9][0-9]*$");

        public string this[string columnName]
        {
            get
            {
                string? error = null;
                string requiredMessage = "Required field";
                switch (columnName)
                {
                    case nameof(Duration):
                        if (string.IsNullOrEmpty(Duration)) error = requiredMessage;

                        if (Duration != null)
                        {
                            Match numberMatch = _numberRegex.Match(Duration);
                            if (!numberMatch.Success)
                                return "This field must be a whole number!";
                        }
                        break;
                }
                return error;

            }
        }
        public string Error => null;
        public bool IsValid
        {
            get
            {
                foreach (var property in new string[]
                {
                    nameof(Duration)})
                {
                    if (this[property] != null) return false;
                }
                return true;
            }

        }
    }
}
