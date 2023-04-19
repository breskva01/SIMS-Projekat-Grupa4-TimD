using InitialProject.Domain.Models;
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
        private bool _anyNotifications;
        public bool AnyNotifications
        {
            get => _anyNotifications;
            set
            {
                if (_anyNotifications != value)
                {
                    _anyNotifications = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TypeSelectedIndex { get; set; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand ShowReservationViewCommand { get; }
        public ICommand ShowMyReservationsViewCommand { get; }
        public ICommand ShowRatingsViewCommand { get; }
        public ICommand ShowRequestsViewCommand { get; }
        public ICommand SortByNameCommand { get; }
        public ICommand SortByLocationCommand { get; }
        public ICommand SortByMaxGuestNumberCommand { get; }
        public ICommand SortByMinDaysNumberCommand { get; }
        public ICommand GuestNumberIncrementCommand { get; }
        public ICommand NumberOfDaysIncrementCommand { get; }
        public ICommand GuestNumberDecrementCommand { get; }
        public ICommand NumberOfDaysDecrementCommand { get; }
        public ICommand NewNotificationsCommand { get; }
        private string _lastSortingCriterium;
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
            GuestNumberDecrementCommand = new ExecuteMethodCommand(DecrementGuestNumber);
            NumberOfDaysDecrementCommand = new ExecuteMethodCommand(DecrementNumberOfDays);
            ShowReservationViewCommand = new AccommodationClickCommand(ShowAccommodationReservationView);
            ShowMyReservationsViewCommand = new ExecuteMethodCommand(ShowMyReservationsView);
            ShowRatingsViewCommand = new ExecuteMethodCommand(ShowRatingsView);
            ShowRequestsViewCommand = new ExecuteMethodCommand(ShowRequestsView);
            NewNotificationsCommand = new ExecuteMethodCommand(NotificationsPrompt);

            CheckForNotifications();
        }
        private void CheckForNotifications()
        {
            var requestService = new AccommodationReservationRequestService();
            if (requestService.GetAllNewlyAnswered(_loggedInUser.Id).Count > 0)
            {
                AnyNotifications = true;
            }
            else
                AnyNotifications = false;
        }
        private void NotificationsPrompt()
        {
            var requestService = new AccommodationReservationRequestService();
            requestService.UpdateGuestNotifiedField(_loggedInUser.Id);
            MessageBoxResult result = MessageBox.Show(
                   "Stiglo je jedan ili više novih odgovora na vaše zahteve," +
                   "da li želite da ih pogledate?",
                   "Obaveštenje", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                ShowRequestsView();
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
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
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
            List<Accommodation> sortedAccommodations;
            if (_lastSortingCriterium == criterium)
                 sortedAccommodations = new List<Accommodation>(Accommodations.Reverse());
            else
            {
                sortedAccommodations = _service.Sort(new List<Accommodation>(Accommodations), criterium);
                _lastSortingCriterium = criterium;
            }          
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
        private void ShowRatingsView()
        {
            var viewModel = new AccommodationRatingViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
        private void ShowRequestsView()
        {
            var viewModel = new MyAccommodationReservationRequestsViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
