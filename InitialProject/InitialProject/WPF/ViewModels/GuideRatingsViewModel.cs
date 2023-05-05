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
    public class GuideRatingsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private List<TourRating> _tourRatings;
        private List<TourReservation> _tourReservations;
        private readonly ObservableCollection<Tour> _toursShow;
        public IEnumerable<Tour> ToursShow => _toursShow;

        private TourService _tourService;
        private LocationService _locationService;
        private TourRatingService _ratingService;
        private TourReservationService _reservationService;

        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<RatingViewModel> Ratings { get; set; }

        public ICommand ChooseTourCommand { get; set; }
        public ICommand ReportGuestRatingCommand { get; set; }
        public ICommand BackCommand { get; set; }

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

        private RatingViewModel _selectedGuestRating;
        public RatingViewModel SelectedGuestRating
        {
            get { return _selectedGuestRating; }
            set
            {
                _selectedGuestRating = value;
                OnPropertyChanged(nameof(SelectedGuestRating));

            }
        }

        public GuideRatingsViewModel(NavigationStore navigationStore, User user) 
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourService = new TourService();
            _reservationService = new TourReservationService();
            _locationService = new LocationService();
            _ratingService = new TourRatingService();
            
            _tourRatings = new List<TourRating>(_ratingService.GetAll());
            _toursShow = new ObservableCollection<Tour>(_tourService.GetFinishedTours());
            _tourReservations = new List<TourReservation>();    
            Ratings = new ObservableCollection<RatingViewModel>();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());

            foreach (Tour t in _toursShow)
            {
                foreach (Location l in Locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }

            InitializeCommands();

        }
        private void InitializeCommands()
        {

            ChooseTourCommand = new ExecuteMethodCommand(ShowRatings);
            ReportGuestRatingCommand = new ExecuteMethodCommand(ReportRating);
            BackCommand = new ExecuteMethodCommand(ShowGuideMenuView);
        }
        private void ShowRatings()
        {
            _tourReservations = _reservationService.GetRatedByTourId(SelectedTour.Id);
            foreach(TourReservation tourReservation in _tourReservations)
            {
                Ratings.Add(new RatingViewModel(tourReservation));
            }
        }
        private void ReportRating()
        {
            SelectedGuestRating.IsValid = false;
            foreach(TourRating tourRating in _tourRatings)
            {
                if(tourRating.Id == SelectedGuestRating.Id)
                {
                    tourRating.IsValid = false;
                    _ratingService.Update(tourRating);
                }
            }
        }

        private void ShowGuideMenuView()
        {
            GuideMenuViewModel viewModel = new GuideMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
    }
}
