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

        private TourService _tourService;

        private readonly ObservableCollection<Tour> _toursShow;
        public IEnumerable<Tour> ToursShow => _toursShow;

        public ObservableCollection<Location> Locations { get; set; }
        private LocationService _locationService;

        public ICommand ChooseTourCommand { get; set; }
        public ICommand ReportGuestRatingCommand { get; set; }


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

        private List<TourReservation> _tourReservations;
        private TourReservationService _reservationService;

        private TourRatingService _ratingService;
        private List<TourRating> _tourRatings;

        //private readonly ObservableCollection<RatingViewModel> _ratings;
        public ObservableCollection<RatingViewModel> Ratings { get; set; }

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

            ChooseTourCommand = new ExecuteMethodCommand(ShowRatings);
            ReportGuestRatingCommand = new ExecuteMethodCommand(ReportRating);

            // tura se selktuje
            // rezervacije preko tour.id
            // filtriram ih da imaju Rating  > 0
            // 

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
           // _ratingService.Update(SelectedGuestRating.TourRating);
        }
    }
}
