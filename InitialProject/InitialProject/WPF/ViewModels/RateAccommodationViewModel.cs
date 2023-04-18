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
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class RateAccommodationViewModel : ViewModelBase
    {
        public AccommodationReservation Reservation { get; set; }
        private readonly AccommodationRatingService _service;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }
        public ICommand ShowRatingsViewCommand { get; }
        public int Location { get; set; }
        public int Hygiene { get; set; }
        public int Pleasantness { get; set; }
        public int Fairness { get; set; }
        public int Parking { get; set; }
        public string Comment { get; set; }
        public RateAccommodationViewModel(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            _navigationStore = navigationStore;
            Reservation = reservation;
            _service = new AccommodationRatingService();
            RateReservationCommand = new ExecuteMethodCommand(SubmitRating);
            ShowRatingsViewCommand = new ExecuteMethodCommand(ShowRatingsView);
        }
        public void SubmitRating()
        {
            MessageBoxResult result = MessageBox.Show
                ("Da li ste sigurni da želite da ocenite rezervaciju?", "Potvrda ocene",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _service.Save(Reservation, Location, Hygiene, Pleasantness, Fairness, Parking, Comment);
                ShowRatingsView();
            }      
        }
        private void ShowRatingsView()
        {
            var viewModel = new AccommodationRatingViewModel(_navigationStore, Reservation.Guest);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
