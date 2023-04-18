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
    public class AccommodationRatingViewModel : ViewModelBase, IObserver
    {
        private readonly User _loggedInUser;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationReservationService _service;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }      
        public ICommand ShowAccommodationBrowserViewCommand { get; }
        public AccommodationRatingViewModel(NavigationStore navigationStore, User loggedInUser)
        {
            _navigationStore = navigationStore;
            _loggedInUser = loggedInUser;
            _service = new AccommodationReservationService();
            Reservations = new ObservableCollection<AccommodationReservation>
                                (_service.GetEgligibleForRating(_loggedInUser.Id));
            _service.Subscribe(this);
            RateReservationCommand = new AccommodationReservationClickCommand(ShowRateAccommodationView);
            ShowAccommodationBrowserViewCommand = new ExecuteMethodCommand(ShowAccommodationBrowserView);
        }
        private void ShowRateAccommodationView(AccommodationReservation reservation)
        {
            var viewModel = new RateAccommodationViewModel(_navigationStore, reservation);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
        public void Update()
        {
            var reservations = new ObservableCollection<AccommodationReservation>
                                (_service.GetEgligibleForRating(_loggedInUser.Id));
            Reservations.Clear();
            foreach (var r in reservations)
                Reservations.Add(r);
        }
        private void ShowAccommodationBrowserView()
        {
            var viewModel = new AccommodationBrowserViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
