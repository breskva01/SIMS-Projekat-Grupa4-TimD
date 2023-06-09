using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace InitialProject.WPF.ViewModels
{
    public class TourLiveTrackingViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user; 
        private Tour _tour;

        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<KeyPoint> KeyPoints { get; set; }

        private List<TourReservation> _tourReservations { get; set; }

        private KeyPointService _keyPointService;
        private TourService _tourService;
        private LocationService _locationService;
        private TourReservationService _tourReservationService;

        private readonly ObservableCollection<KeyPointViewModel> _keyPointsFromSelectedTour;
        public IEnumerable<KeyPointViewModel> KeyPointsFromSelectedTour => _keyPointsFromSelectedTour;

        public ICommand GuestAtTourNavigateCommand =>
          new NavigateCommand(new NavigationService(_navigationStore, ShowTourGuests()));

        public ICommand BackNavigationCommand =>
           new NavigateCommand(new NavigationService(_navigationStore, GoBack()));

        private string _tourName;
        public string TourName
        {
            get { return _tourName; }
            set
            {
                _tourName = value;
                OnPropertyChanged(nameof(TourName));

            }
        }

        public ICommand KeyPointReachedCommand { get; set; }
        public ICommand StopTourCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand CreateTourCommand { get; set; }
        public ICommand LiveTrackingCommand { get; set; }
        public ICommand CancelTourCommand { get; set; }
        public ICommand TourStatsCommand { get; set; }
        public ICommand RatingsViewCommand { get; set; }
        public ICommand RequestsStatsCommand { get; set; }
        public ICommand RequestsCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private KeyPointViewModel _selectedKeyPoint;
        public KeyPointViewModel SelectedKeyPoint
        {
            get { return _selectedKeyPoint; }
            set
            {
                _selectedKeyPoint = value;
                OnPropertyChanged(nameof(SelectedKeyPoint));

            }
        }

        public TourLiveTrackingViewModel(NavigationStore navigationStore, User user, Tour tour) 
        {
            _navigationStore = navigationStore;
            _user = user;
            _tour = tour;

            _tourService = new TourService();
            _locationService = new LocationService();
            _keyPointService = new KeyPointService();
            _tourReservationService = new TourReservationService();

            _tourReservations = new List<TourReservation>();
            _keyPointsFromSelectedTour = new ObservableCollection<KeyPointViewModel>();

            TourName = "Tour Name: ";

            TourName += _tour.Name;

            KeyPoints = new ObservableCollection<KeyPoint>(_keyPointService.GetAll());
            Locations = new ObservableCollection<Location>(_locationService.GetAll());

            foreach (int keyPointId in _tour.KeyPointIds)
            {
                foreach (KeyPoint ky in KeyPoints)
                {
                    if (ky.Id == keyPointId)
                    {
                        KeyPointViewModel keyPointViewModel = new KeyPointViewModel(ky);
                        _keyPointsFromSelectedTour.Add(keyPointViewModel);
                    }
                }
            }
            
            foreach (KeyPointViewModel keyPoint in _keyPointsFromSelectedTour)
            {
                keyPoint.Location = _locationService.GetById(keyPoint.LocationId);
            }

            if(_tour.CurrentKeyPoint == 0)
            {
                _keyPointsFromSelectedTour[0].IsReached = true;
                _tour.CurrentKeyPoint = _keyPointsFromSelectedTour[0].KeyPointId;
                _tourService.Update(_tour);
            }

            int index = _keyPointsFromSelectedTour.IndexOf(_keyPointsFromSelectedTour.FirstOrDefault(l => l.KeyPointId == _tour.CurrentKeyPoint));

            for (int i = 0; i <= index; i++)
            {
                _keyPointsFromSelectedTour[i].IsReached = true;
            }

            InitializeCommands();

        }
        private void InitializeCommands()
        {
            KeyPointReachedCommand = new ExecuteMethodCommand(KeyPointReached);
            StopTourCommand = new ExecuteMethodCommand(StopTour);
            HomeCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            CreateTourCommand = new ExecuteMethodCommand(ShowTourCreationView);
            LiveTrackingCommand = new ExecuteMethodCommand(ShowToursTodayView);
            CancelTourCommand = new ExecuteMethodCommand(ShowTourCancellationView);
            TourStatsCommand = new ExecuteMethodCommand(ShowTourStatsView);
            RatingsViewCommand = new ExecuteMethodCommand(ShowGuideRatingsView);
            RequestsCommand = new ExecuteMethodCommand(ShowRequestsView);
            RequestsStatsCommand = new ExecuteMethodCommand(ShowRequestsStatsView);
            BackCommand = new ExecuteMethodCommand(ShowPreviousWindow);

        }

        private void KeyPointReached()
        {
            SelectedKeyPoint.IsReached = true;
             
            _tour.CurrentKeyPoint = SelectedKeyPoint.KeyPointId;
            int index = _keyPointsFromSelectedTour.IndexOf(_keyPointsFromSelectedTour.FirstOrDefault(l => l.KeyPointId == _tour.CurrentKeyPoint));

            if (++index== _keyPointsFromSelectedTour.Count())
            {
                _tour.State = TourState.Finished;
                _tourReservations = _tourReservationService.GetPresentByTourId(_tour.Id);
                foreach(TourReservation tourReservation in _tourReservations)
                {
                    tourReservation.RatingId = 0;
                    _tourReservationService.Update(tourReservation);
                }
                
                _tourService.Update(_tour);
                GuestAtTourNavigateCommand.Execute(null);
                return;
            }
            _tourService.Update(_tour);
            GuestAtTourNavigateCommand.Execute(null);

        }

        private void StopTour()
        {
            _tour.State = TourState.Interrupted;
            _tourService.Update(_tour);

            BackNavigationCommand.Execute(null);
        }
        private TourGuestsViewModel ShowTourGuests()
        {
            return new TourGuestsViewModel(_navigationStore, _user, _tour);

        }
        private ToursTodayViewModel GoBack()
        {
            return new ToursTodayViewModel(_navigationStore, _user);

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
        private void ShowRequestsStatsView()
        {
            TourRequestsStatsViewModel viewModel = new TourRequestsStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
        private void ShowRequestsView()
        {
            TourRequestsAcceptViewModel viewModel = new TourRequestsAcceptViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
        private void ShowPreviousWindow()
        {
            BackNavigationCommand.Execute(null);
        }

    }
}
