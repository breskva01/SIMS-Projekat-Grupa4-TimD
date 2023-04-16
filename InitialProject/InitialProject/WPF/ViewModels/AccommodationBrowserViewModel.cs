using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows;
using InitialProject.Application.Services;
using InitialProject.Application.Commands;
using InitialProject.Application.Stores;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationBrowserViewModel : ViewModelBase
    {
        private readonly User _loggedInUser;
        private readonly NavigationStore _navigationStore;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        private readonly AccommodationService _service;
        private int _guestNumber;
        public int GuestNumber
        {
            get { return _guestNumber; }
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    CanDecrementGuestNumber = value > 1;
                    OnPropertyChanged();
                }
            }
        }
        private bool _canDecrementGuestNumber;
        public bool CanDecrementGuestNumber
        {
            get => _canDecrementGuestNumber;
            set
            {
                if (_canDecrementGuestNumber != value)
                {
                    _canDecrementGuestNumber = value;
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
                    CanDecrementNumberOfDays = value > 1;
                    OnPropertyChanged();
                }
            }
        }
        private bool _canDecrementNumberOfDays;
        public bool CanDecrementNumberOfDays
        {
            get => _canDecrementNumberOfDays;
            set
            {
                if (_canDecrementNumberOfDays != value)
                {
                    _canDecrementNumberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TypeSelectedIndex { get; set; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand ShowReservationViewCommand { get; }
        public ICommand ShowMyReservationsViewCommand { get; }
        public ICommand SortByNameCommand { get; }
        public ICommand SortByLocationCommand { get; }
        public ICommand SortByMaxGuestNumberCommand { get; }
        public ICommand SortByMinDaysNumberCommand { get; }
        public ICommand GuestNumberIncrementCommand { get; }
        public ICommand NumberOfDaysIncrementCommand { get; }
        public ICommand GuestNumberDecrementCommand { get; }
        public ICommand NumberOfDaysDecrementCommand { get; }
        public AccommodationBrowserViewModel(NavigationStore navigationStore ,User user)
        {
            _loggedInUser = user;
            _navigationStore = navigationStore;
            _service = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_service.GetAll());
            GuestNumber = 1;
            NumberOfDays = 1;
            ApplyFiltersCommand = new ExecuteMethodCommand(ApplyFilters);
            ResetFiltersCommand = new ExecuteMethodCommand(ResetFilters);
            SortByNameCommand = new SortAccommodationsCommand(SortAccommodations, "Name");
            SortByLocationCommand = new SortAccommodationsCommand(SortAccommodations, "Location");
            SortByMaxGuestNumberCommand = new SortAccommodationsCommand(SortAccommodations, "MaxGuestNumber");
            SortByMinDaysNumberCommand = new SortAccommodationsCommand(SortAccommodations, "MinDaysNumber");
            GuestNumberIncrementCommand = new ExecuteMethodCommand(IncrementGuestNumber);
            NumberOfDaysIncrementCommand = new ExecuteMethodCommand(IncrementNumberOfDays);
            GuestNumberDecrementCommand = new DecrementCommand(DecrementGuestNumber);
            NumberOfDaysDecrementCommand = new DecrementCommand(DecrementNumberOfDays);
            ShowReservationViewCommand = new AccommodationClickCommand(ShowAccommodationReservationView);
            ShowMyReservationsViewCommand = new ExecuteMethodCommand(ShowMyReservationsView);
        }
        private void ApplyFilters()
        {
            AccommodationType type = GetSelectedType();

            Accommodations.Clear();
            foreach (var accommodation in _service.GetFiltered(SearchText, type, GuestNumber, NumberOfDays))
            {
                Accommodations.Add(accommodation);
            }
        }
        private AccommodationType GetSelectedType()
        {
            switch (TypeSelectedIndex)
            {
                case 0:
                    return AccommodationType.Everything;
                case 1:
                    return AccommodationType.House;
                case 2:
                    return AccommodationType.Apartment;
                default:
                    return AccommodationType.Cottage;
            }
        }
        private void ResetFilters()
        {
            SearchText = "";
            GuestNumber = 1;
            NumberOfDays = 1;
            TypeSelectedIndex = 0;
            Accommodations.Clear();
            foreach (var accommodation in _service.GetAll())
            {
                Accommodations.Add(accommodation);
            }
        }

        private void ShowAccommodationReservationView(Accommodation accommodation)
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, _loggedInUser, accommodation);
            var reserveAccommodationNavigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            reserveAccommodationNavigateCommand.Execute(null);
        }

        private void DecrementGuestNumber()
        {
            GuestNumber--;
        }

        private void IncrementGuestNumber()
        {
            GuestNumber++;
        }

        private void DecrementNumberOfDays()
        {
            NumberOfDays--;
        }

        private void IncrementNumberOfDays()
        {
            NumberOfDays++;
        }

        private void SortAccommodations(string criterium)
        {
            var sortedAccommodations = _service.Sort(new List<Accommodation>(Accommodations), criterium);
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void ShowMyReservationsView()
        {
            var viewModel = new MyAccommodationReservationsViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
