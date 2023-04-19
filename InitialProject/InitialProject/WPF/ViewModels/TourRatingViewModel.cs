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
    public class TourRatingViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourReservationService _tourReservationService;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<Tour> UnratedTours { get; set; }

        public ICommand RateTourCommand { get; }
        public ICommand MenuCommand { get; }

        public TourRatingViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _locationService = new LocationService();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            List<Tour> Tours = new List<Tour>();

            List<TourReservation> unratedReservations = _tourReservationService.GetUnratedByUser(user.Id);
            foreach(TourReservation tr in unratedReservations)
            {
                Tour tour = _tourService.GetById(tr.TourId);
                tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }
            UnratedTours = new ObservableCollection<Tour>(Tours.DistinctBy(t => t.Id));



            RateTourCommand = new TourClickCommand(ShowRateTourView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);

        }
        
        private void ShowRateTourView(Tour tour)
        {
            RateTourViewModel rateTourViewModel= new RateTourViewModel(_navigationStore, _user, tour);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, rateTourViewModel));
            navigateCommand.Execute(null);
        }
        

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

    }
}
