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
using System.Windows;
using InitialProject.Application.Observer;

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class AccommodationRatingViewModel : ViewModelBase
    {
        private readonly User _user;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationRatingService _ratingService;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }  
        public AccommodationRatingViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _ratingService = new AccommodationRatingService();
            Reservations = new ObservableCollection<AccommodationReservation>
                                (_ratingService.GetEligibleForRating(_user.Id));
            RateReservationCommand = new AccommodationReservationClickCommand(NavigateRateAccommodation);
        }
        private void NavigateRateAccommodation(AccommodationReservation reservation)
        {
            var viewModel = new RateAccommodationViewModel(_navigationStore, reservation);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
