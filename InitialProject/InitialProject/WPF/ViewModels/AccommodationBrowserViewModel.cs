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
        private readonly AccommodationService _service;
        private string _lastSortingCriterion;
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
        public AccommodationType[] AccommodationTypes { get; } = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>().ToArray();

        private AccommodationType _selectedAccommodationType;
        public AccommodationType SelectedAccommodationType
        {
            get => _selectedAccommodationType;
            set
            {
                if (_selectedAccommodationType != value)
                {
                    _selectedAccommodationType = value;
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
        public ICommand GuestCountIncrementCommand { get; }
        public ICommand NumberOfDaysIncrementCommand { get; }
        public ICommand GuestCountDecrementCommand { get; }
        public ICommand NumberOfDaysDecrementCommand { get; }
        public ICommand NewNotificationsCommand { get; }
        public ICommand SignOutCommand { get; }
        public AccommodationBrowserViewModel(NavigationStore navigationStore ,User user)
        {
            _loggedInUser = user;
            _navigationStore = navigationStore;
            _service = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_service.GetAll());
            GuestCount = 1;
            NumberOfDays = 1;
            ApplyFiltersCommand = new ExecuteMethodCommand(ApplyFilters);
            ResetFiltersCommand = new ExecuteMethodCommand(ResetFilters);
            SortByNameCommand = new SortAccommodationsCommand(SortAccommodations, "Name");
            SortByLocationCommand = new SortAccommodationsCommand(SortAccommodations, "Location");
            SortByMaxGuestNumberCommand = new SortAccommodationsCommand(SortAccommodations, "MaxGuestNumber");
            SortByMinDaysNumberCommand = new SortAccommodationsCommand(SortAccommodations, "MinDaysNumber");
            GuestCountIncrementCommand = new IncrementCommand(() => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysIncrementCommand = new IncrementCommand(() => NumberOfDays, (newValue) => NumberOfDays = newValue);
            GuestCountDecrementCommand = new DecrementCommand(this, () => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysDecrementCommand = new DecrementCommand(this, () => NumberOfDays, (newValue) => NumberOfDays = newValue);
            ShowReservationViewCommand = new AccommodationClickCommand(ShowAccommodationReservationView);
            ShowMyReservationsViewCommand = new ExecuteMethodCommand(ShowMyReservationsView);
            ShowRatingsViewCommand = new ExecuteMethodCommand(ShowRatingsView);
            ShowRequestsViewCommand = new ExecuteMethodCommand(ShowRequestsView);
            NewNotificationsCommand = new ExecuteMethodCommand(NotificationsPrompt);
            SignOutCommand = new ExecuteMethodCommand(ShowSignInView);
            SelectedAccommodationType = AccommodationType.Everything;
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
            Accommodations.Clear();
            _service.GetFiltered(SearchText, SelectedAccommodationType, GuestCount, NumberOfDays).
                ForEach(a =>  Accommodations.Add(a));
        }
        private void ResetFilters()
        {
            SearchText = "";
            GuestCount = NumberOfDays = 1;
            SelectedAccommodationType = AccommodationType.Everything;
            Accommodations.Clear();
            _service.GetAll().ForEach(a => Accommodations.Add(a));
        }
        private void SortAccommodations(string criterion)
        {
            List<Accommodation> sortedAccommodations;
            if (_lastSortingCriterion == criterion)
                sortedAccommodations = new List<Accommodation>(Accommodations.Reverse());
            else
            {
                sortedAccommodations = _service.Sort(Accommodations, criterion);
                _lastSortingCriterion = criterion;
            }
            Accommodations.Clear();
            sortedAccommodations.ForEach(a => Accommodations.Add(a));
        }
        private void ShowAccommodationReservationView(Accommodation accommodation)
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, _loggedInUser, accommodation);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
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
        private void ShowSignInView()
        {
            var viewModel = new SignInViewModel(_navigationStore);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
