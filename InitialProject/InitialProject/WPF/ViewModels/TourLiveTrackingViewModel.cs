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
        public ObservableCollection<Location> Locations { get; set; }
        private LocationService _locationService;
        public ObservableCollection<KeyPoint> KeyPoints { get; set; }

        private TourService _tourService;

        private readonly ObservableCollection<KeyPointViewModel> _keyPointsFromSelectedTour;
        public IEnumerable<KeyPointViewModel> KeyPointsFromSelectedTour => _keyPointsFromSelectedTour;
        
        private Tour _tour;
        private KeyPointService _keyPointService;

        private int _numberOfKeyPointsFromSelectedTour;


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


        private readonly NavigationStore _navigationStore;
        private User _user;

        public ICommand KeyPointReachedCommand { get; set; }

        public TourLiveTrackingViewModel(NavigationStore navigationStore, User user, Tour tour) 
        {
            _tour = new Tour();
            _tour = tour;

            _tourService = new TourService();

            _navigationStore = navigationStore;
            _user = user;

            _locationService = new LocationService();
            _keyPointService = new KeyPointService();


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
                keyPoint.Location = _locationService.Get(keyPoint.LocationId);
            }

            _keyPointsFromSelectedTour[0].IsReached = true;
            int index = _keyPointsFromSelectedTour.IndexOf(_keyPointsFromSelectedTour.FirstOrDefault(l => l.KeyPointId == _tour.CurrentKeyPoint));

            for (int i = 0; i <= index; i++)
            {
               _keyPointsFromSelectedTour[i].IsReached = true;
            }

            _numberOfKeyPointsFromSelectedTour = 1;

            InitializeCommands();

        }
        private void InitializeCommands()
        {
            KeyPointReachedCommand = new ExecuteMethodCommand(KeyPointReached);
        }
       
        public ICommand GuestAtTourNavigateCommand =>
           new NavigateCommand(new NavigationService(_navigationStore, ShowTourGuests()));

        private void KeyPointReached()
        {
            SelectedKeyPoint.IsReached = true;
            
            
            _tour.CurrentKeyPoint = SelectedKeyPoint.KeyPointId;
            int index = _keyPointsFromSelectedTour.IndexOf(_keyPointsFromSelectedTour.FirstOrDefault(l => l.KeyPointId == _tour.CurrentKeyPoint));

            if (++index== _keyPointsFromSelectedTour.Count())
            {
                _tour.State = TourState.Finished;
                _tourService.Update(_tour);
                GuestAtTourNavigateCommand.Execute(null);
                return;
            }
            _tourService.Update(_tour);
            GuestAtTourNavigateCommand.Execute(null);


        }
        private TourGuestsViewModel ShowTourGuests()
        {
            return new TourGuestsViewModel(_navigationStore, _user);

        }

    }
}
