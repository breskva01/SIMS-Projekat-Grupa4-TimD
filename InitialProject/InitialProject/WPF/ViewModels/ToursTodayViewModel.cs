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
    public class ToursTodayViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Tour> _tours;
        private readonly ObservableCollection<Tour> _toursToday;
        public IEnumerable<Tour> ToursToday => _toursToday;

        public ObservableCollection<Location> Locations { get; set; }
        private LocationService _locationService;

        private DateTime _today;

        public int NumberOfActiveTours;

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

        private TourService _tourService;

        private readonly NavigationStore _navigationStore;
        private User _user;

        public ICommand StartTourCommand { get; set; }
        public ICommand BackCommand { get; set; }
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
        public ICommand SignOutCommand { get; set; }

        public ToursTodayViewModel(NavigationStore navigationStore, User user)
        {
            _today = DateTime.Now;
            _toursToday = new ObservableCollection<Tour>();

            _navigationStore = navigationStore;
            _user = user;
            _tourService = new TourService();
            _locationService = new LocationService();
            _tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new ObservableCollection<Location>(_locationService.GetAll());

            foreach (Tour t in _tours)
            {
                foreach (Location l in Locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }

            foreach (Tour t in _tours)
            {
                if (IsDateToday(t) && (t.State == TourState.None || t.State == TourState.Started))
                {
                    _toursToday.Add(t);
                }
            }

            NumberOfActiveTours = 0;

            InitializeCommands();

        }
        private void InitializeCommands()
        {
            StartTourCommand = new ExecuteMethodCommand(StartTour);
            BackCommand = new ExecuteMethodCommand(ShowGuideMenuView);
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
            SignOutCommand = new ExecuteMethodCommand(SignOut);
        }
        private bool IsDateToday(Tour t)
        {
            return t.Start.Year == _today.Year && t.Start.Month == _today.Month && t.Start.Day == _today.Day;
        }

        public ICommand LiveTrackingNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, LiveTracking()));

        private void StartTour()
        {
            if(SelectedTour != null)
            {
                if(NumberOfActiveTours != 0)
                {
                    return;
                }
                if(SelectedTour.State == (TourState)TourState.None || SelectedTour.State == (TourState)TourState.Started)
                {
                    SelectedTour.State = (TourState)TourState.Started;
                    _tourService.Update(SelectedTour);
                    NumberOfActiveTours++;
                    LiveTrackingNavigateCommand.Execute(null);
                    SelectedTour = null;
                    return;
                }
            }
            return;
        }
        private TourLiveTrackingViewModel LiveTracking()
        {
            return new TourLiveTrackingViewModel(_navigationStore, _user, SelectedTour);

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


    }
}
