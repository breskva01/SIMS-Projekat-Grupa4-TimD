using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class ScheduleRenovationViewModel : ViewModelBase
    {
        private Accommodation _selectedAccommodation;
        private DateTime _start;
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
        private DateTime _end;
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
        private string _duration;
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
            var availableDates = _acommodationReservationService.GetAvailableDates(Start, End, int.Parse(Duration)-1, _selectedAccommodation.Id);
            AvailableDates.Clear();
            foreach (var date in availableDates)
            {
                AvailableDates.Add(date);
            }
        }
        private void ScheduleRenovation()
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
}
