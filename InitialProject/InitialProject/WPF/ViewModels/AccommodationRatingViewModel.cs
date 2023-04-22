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

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationRatingViewModel : ViewModelBase
    {
        private readonly User _loggedInUser;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationRatingService _service;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }      
        public ICommand ShowAccommodationBrowserViewCommand { get; }
        public AccommodationRatingViewModel(NavigationStore navigationStore, User loggedInUser)
        {
            _navigationStore = navigationStore;
            _loggedInUser = loggedInUser;
            _service = new AccommodationRatingService();
            Reservations = new ObservableCollection<AccommodationReservation>
                                (_service.GetEligibleForRating(_loggedInUser.Id));
            RateReservationCommand = new AccommodationReservationClickCommand(ShowRateAccommodationView);
            ShowAccommodationBrowserViewCommand = new ExecuteMethodCommand(ShowAccommodationBrowserView);
        }
        private void ShowRateAccommodationView(AccommodationReservation reservation)
        {
            var viewModel = new RateAccommodationViewModel(_navigationStore, reservation);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
        private void ShowAccommodationBrowserView()
        {
            var viewModel = new AccommodationBrowserViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
