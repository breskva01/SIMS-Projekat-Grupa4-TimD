using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
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
    public class TourGuestsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;
        private Tour _tour;

        private TourReservationService _tourReservationService;
        private UserService _userService;
        private TourService _tourService;

        private readonly ObservableCollection<TourReservation> _tourReservations;
        private readonly ObservableCollection<User> _users;
        private readonly ObservableCollection<User> _guests;
        public IEnumerable<User> Guests => _guests;

        private User _selectedGuest;
        public User SelectedGuest
        {
            get { return _selectedGuest; }
            set
            {
                _selectedGuest = value;
                OnPropertyChanged(nameof(SelectedGuest));
                GuestSelected();
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand CreateTourCommand { get; set; }
        public ICommand LiveTrackingCommand { get; set; }
        public ICommand CancelTourCommand { get; set; }
        public ICommand TourStatsCommand { get; set; }
        public ICommand RatingsViewCommand { get; set; }
        public ICommand TourRequestsCommand { get; set; }
        public ICommand TourRequestsStatsCommand { get; set; }
        public ICommand GuideProfileCommand { get; set; }
        public ICommand ComplexTourCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public ICommand BackNavigationCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, GoBack()));

        public TourGuestsViewModel(NavigationStore navigationStore, User user, Tour tour)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tour = tour;

            _tourReservationService = new TourReservationService();
            _userService = new UserService();
            _tourService = new TourService();

            _guests = new ObservableCollection<User>();

            _tourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAll());
            _users = new ObservableCollection<User>(_userService.GetAll());

            foreach (TourReservation res in _tourReservations)
            {
                foreach (User u in _users)
                {
                    if (res.GuestId == u.Id && !_guests.Contains(u) && res.Presence == Presence.Absent && res.TourId == tour.Id)
                    {
                        _guests.Add(u);
                    }
                }
            }

            BackCommand = new ExecuteMethodCommand(ShowPreviousWindow);
            HomeCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            CreateTourCommand = new ExecuteMethodCommand(ShowTourCreationView);
            LiveTrackingCommand = new ExecuteMethodCommand(ShowToursTodayView);
            CancelTourCommand = new ExecuteMethodCommand(ShowTourCancellationView);
            TourStatsCommand = new ExecuteMethodCommand(ShowTourStatsView);
            RatingsViewCommand = new ExecuteMethodCommand(ShowGuideRatingsView);
            TourRequestsCommand = new ExecuteMethodCommand(ShowTourRequestsView);
            TourRequestsStatsCommand = new ExecuteMethodCommand(ShowTourRequestsStatsView);
            GuideProfileCommand = new ExecuteMethodCommand(ShowGuideProfileView);
            ComplexTourCommand = new ExecuteMethodCommand(ShowComplexTourView);
            BackCommand = new ExecuteMethodCommand(ShowPreviousWindow);

        }

        private TourLiveTrackingViewModel GoBack()
        {
            return new TourLiveTrackingViewModel(_navigationStore, _user, _tour);
        }

        private void GuestSelected()
        {

            foreach (TourReservation res in _tourReservations)
            {
                if (res.GuestId == SelectedGuest.Id && res.TourId == _tour.Id)
                {
                    //_tour.NumberOfArrivedGeusts += res.NumberOfGuests;
                    _tourService.Update(_tour);
                    res.Presence = Presence.Pending;
                    res.ArrivedAtKeyPoint = _tour.CurrentKeyPoint;
                    _tourReservationService.Update(res);
                }
            }
        }
        private void ShowGuideMenuView()
        {
            GuideMenuViewModel viewModel = new GuideMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
        private void ShowTourCreationView()
        {
            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }

        private void ShowComplexTourView()
        {
            ComplexTourAcceptViewModel viewModel = new ComplexTourAcceptViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowToursTodayView()
        {
            ToursTodayViewModel viewModel = new ToursTodayViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourCancellationView()
        {
            AllToursViewModel viewModel = new AllToursViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourStatsView()
        {
            TourStatsViewModel viewModel = new TourStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowGuideRatingsView()
        {
            GuideRatingsViewModel viewModel = new GuideRatingsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourRequestsView()
        {
            TourRequestsAcceptViewModel viewModel = new TourRequestsAcceptViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourRequestsStatsView()
        {
            TourRequestsStatsViewModel viewModel = new TourRequestsStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowGuideProfileView()
        {
            GuideProfileViewModel viewModel = new GuideProfileViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowPreviousWindow()
        {
            BackNavigationCommand.Execute(null);
        }
    }
}
