using InitialProject.Application.Commands;
using InitialProject.Application.Factories;
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

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class UnratedAccommodationsViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationRatingService _ratingService;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }
        public UnratedAccommodationsViewModel(NavigationStore navigationStore, Guest1 user)
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
            var viewModel = ViewModelFactory.Instance.CreateRateAccommodationVM(_navigationStore, reservation);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
