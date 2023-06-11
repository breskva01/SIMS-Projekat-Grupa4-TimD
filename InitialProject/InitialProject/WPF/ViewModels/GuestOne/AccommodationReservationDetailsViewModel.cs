using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class AccommodationReservationDetailsViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly UserService _userService;
        private readonly NavigationStore _navigationStore;
        public AccommodationReservation Reservation { get; set; }
        public bool IsReadOnlyMode { get; set; }
        public double AccommodationAverageRating { get; set; }
        public double OwnerAverageRating { get; set; }
        public string SecondImagePath => Reservation.Accommodation.PictureURLs.Count > 1 ? Reservation.Accommodation.PictureURLs[1] : null;
        public string ThirdImagePath => Reservation.Accommodation.PictureURLs.Count > 2 ? Reservation.Accommodation.PictureURLs[2] : null;
        public string FourthImagePath => Reservation.Accommodation.PictureURLs.Count > 3 ? Reservation.Accommodation.PictureURLs[3] : null;
        public string FifthImagePath => Reservation.Accommodation.PictureURLs.Count > 4 ? Reservation.Accommodation.PictureURLs[4] : null;

        public Guest1 Guest { get; set; }
        public ICommand NavigateImageBrowserCommand { get; }
        public ICommand MakeReservationCommand { get; }
        public ICommand NavigateAnywhereAnytimeCommand { get; }
        public AccommodationReservationDetailsViewModel(NavigationStore navigationStore, Guest1 user, AccommodationReservation reservation, bool isReadOnlyMode = true)
        {
            _navigationStore = navigationStore;
            Guest = user;
            Reservation = reservation;
            var ratingService = new AccommodationRatingService();
            AccommodationAverageRating = ratingService.CalculateAccommodationAverageRating(reservation.Id);
            OwnerAverageRating = ratingService.CalculateOwnerAverageRating(reservation.Accommodation.Owner.Id);
            _reservationService = new AccommodationReservationService();
            _userService = new UserService();
            MakeReservationCommand = new ExecuteMethodCommand(MakeReservation);
            NavigateAnywhereAnytimeCommand = new ExecuteMethodCommand(NavigateAnywhereAnytime);
            NavigateImageBrowserCommand = new ImageClickCommand(NavigateImageBrowser);
            IsReadOnlyMode = isReadOnlyMode;
        }

        private void MakeReservation()
        {
            _reservationService.Save(Reservation);
            if (_userService.IsDiscountAvailable(Reservation.Guest))
            {
                MessageBox.Show("Rezervacija uspešno kreirana.\n" +
                                "Iskoristili ste jedan bonus poen i time ostvarili popust.\n" +
                                $"Preostalo vam je {Reservation.Guest.BonusPoints} " +
                                "neiskorišćenih bonus poena.");
            }
            else
            {
                MessageBox.Show("Rezervacija uspešno kreirana.");
            }
            NavigateAnywhereAnytime();
        }
        private void NavigateAnywhereAnytime()
        {
            var contentViewModel = new AnywhereAnytimeViewModel(_navigationStore, Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
        private void NavigateImageBrowser(string imageURL)
        {
            var viewModel = new ImageBrowserViewModel(Reservation.Accommodation.PictureURLs, imageURL, RecreateSelf);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
        private void RecreateSelf()
        {
            var viewModel = new AccommodationReservationDetailsViewModel(_navigationStore, Guest, Reservation, IsReadOnlyMode);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
    }
}
