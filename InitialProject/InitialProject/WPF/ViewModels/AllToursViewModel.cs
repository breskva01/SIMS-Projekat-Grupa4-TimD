using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class AllToursViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public ObservableCollection<Location> _locations { get; set; }
        private readonly ObservableCollection<Tour> _tours;
        private readonly ObservableCollection<Tour> _toursShow;
        public IEnumerable<Tour> ToursShow => _toursShow;

        private List<User> _users;
        private List<int> _guestIds;
        private List<User> _guests;
        private List<TourReservation> _tourReservations;

        private LocationService _locationService;
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private UserService _userService;
        //private VoucherService _voucherService; 

        private DateTime _today;

        public ICommand CreateVoucherNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, CreateVoucher()));
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand HomeCommand { get; set; }

        public ICommand CreateTourCommand { get; set; }
        public ICommand LiveTrackingCommand { get; set; }
        public ICommand CancelTourCommand { get; set; }
        public ICommand TourStatsCommand { get; set; }
        public ICommand RatingsViewCommand { get; set; }
        public ICommand SignOutCommand { get; set; }


        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));

            }
        }

        public AllToursViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            //_voucherService = new VoucherService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _userService = new UserService();
            _tourReservationService = new TourReservationService();

            _guestIds = new List<int>();
            _guests = new List<User>();
            _toursShow = new ObservableCollection<Tour>();

            _tourReservations = new List<TourReservation>(_tourReservationService.GetAll());
            _tours = new ObservableCollection<Tour>(_tourService.GetAll());
            _locations = new ObservableCollection<Location>(_locationService.GetAll());
            _users = new List<User>(_userService.GetAll());

            _today = DateTime.Now;

            foreach (Tour t in _tours)
            {
                foreach (Location l in _locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }
            foreach (Tour t in _tours)
            {
                if (t.State == 0)
                {
                    _toursShow.Add(t);
                }
            }

            InitializeCommands();
        }
        private void InitializeCommands()
        {
            CancelCommand = new ExecuteMethodCommand(CancelTour);
            BackCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            HomeCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            CreateTourCommand = new ExecuteMethodCommand(ShowTourCreationView);
            LiveTrackingCommand = new ExecuteMethodCommand(ShowToursTodayView);
            CancelTourCommand = new ExecuteMethodCommand(ShowTourCancellationView);
            TourStatsCommand = new ExecuteMethodCommand(ShowTourStatsView);
            RatingsViewCommand = new ExecuteMethodCommand(ShowGuideRatingsView);
            SignOutCommand = new ExecuteMethodCommand(SignOut);
        }
        private void CancelTour()
        {
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");

                return;
            }
            if (SelectedTour.Start > _today.AddHours(48))
            {
                if (SelectedTour.State == TourState.None)
                {
                    SelectedTour.State = TourState.Canceled;

                    foreach (TourReservation tourReservation in _tourReservations)
                    {
                        if (tourReservation.TourId == SelectedTour.Id)
                        {
                            _guestIds.Add(tourReservation.GuestId);
                        }
                    }

                    foreach (int id in _guestIds)
                    {
                        foreach (User u in _users)
                        {
                            if (id == u.Id && !_guests.Contains(u))
                            {
                                _guests.Add(u);
                            }

                        }
                    }

                    CreateVoucherNavigateCommand.Execute(null);
                    _tourService.Update(SelectedTour);
                    _toursShow.Remove(SelectedTour);
                    return;
                }
            }
            MessageBox.Show("It's too late to cancel this tour.");
            return;
        }
        
        private VoucherCreationViewModel CreateVoucher()
        {
            return new VoucherCreationViewModel(_navigationStore, _user, _guests, 1);

        }

        private void ShowGuideMenuView()
        {
            GuideMenuViewModel viewModel = new GuideMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }
        private void ShowTourCreationView()
        {
            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user);
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


    }
}
