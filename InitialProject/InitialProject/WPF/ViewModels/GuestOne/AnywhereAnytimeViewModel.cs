using InitialProject.Application.Commands;
using InitialProject.Application.Factories;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class AnywhereAnytimeViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationReservationService _reservationService;
        private int _guestCount;
        public int GuestCount
        {
            get { return _guestCount; }
            set
            {
                if (value != _guestCount)
                {
                    _guestCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _numberOfDays;
        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                if (value != _numberOfDays)
                {
                    _numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? EndDate { get; set; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand GuestCountIncrementCommand { get; }
        public ICommand GuestCountDecrementCommand { get; }
        public ICommand NumberOfDaysIncrementCommand { get; }
        public ICommand NumberOfDaysDecrementCommand { get; }
        public ICommand OpenReservationFormCommand { get; }
        public AnywhereAnytimeViewModel(NavigationStore navigationStore, Guest1 user)
        {
            StartDate = DateTime.Now;
            _user = user;
            _navigationStore = navigationStore;
            GuestCount = 1;
            NumberOfDays = 3;
            _reservationService = new AccommodationReservationService();
            Reservations = new ObservableCollection<AccommodationReservation>(
                _reservationService.GetAnywhereAnytime(GuestCount, NumberOfDays, user));
            ApplyFiltersCommand = new ExecuteMethodCommand(ApplyFilters);
            ResetFiltersCommand = new ExecuteMethodCommand(ResetFilters);
            GuestCountIncrementCommand = new IncrementCommand(() => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysIncrementCommand = new IncrementCommand(() => NumberOfDays, (newValue) => NumberOfDays = newValue);
            GuestCountDecrementCommand = new DecrementCommand(this, () => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysDecrementCommand = new DecrementCommand(this, () => NumberOfDays, (newValue) => NumberOfDays = newValue);
            OpenReservationFormCommand = new AccommodationReservationClickCommand(NavigateReservationDetails);
        }

        private void ApplyFilters()
        {
            Reservations.Clear();
            if (EndDate.HasValue)
            {
                _reservationService.GetAnywhereAnytime(GuestCount, NumberOfDays, _user,
                                        DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate.Value)).
                    ForEach(r => Reservations.Add(r));
            }
            else
            {
                _reservationService.GetAnywhereAnytime(GuestCount, NumberOfDays, _user, DateOnly.FromDateTime(StartDate)).
                    ForEach(r => Reservations.Add(r));
            }
        }
        private void ResetFilters()
        {
            GuestCount = 1;
            NumberOfDays = 3;
            StartDate = DateTime.Now;
            EndDate = null;
            Reservations.Clear();
            _reservationService.GetAnywhereAnytime(GuestCount, NumberOfDays, _user).ForEach(r => Reservations.Add(r));
        }
        private void NavigateReservationDetails(AccommodationReservation reservation)
        {
            reservation.GuestCount = GuestCount;
            var viewModel = ViewModelFactory.Instance.CreateReservationDetailsVM(_navigationStore, _user, reservation, false);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
