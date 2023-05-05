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
using System.Collections.Generic;


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

        public ICommand KeyPointReachedCommand { get; set; }
        public ICommand StopTourCommand { get; set; }

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

    }
}
