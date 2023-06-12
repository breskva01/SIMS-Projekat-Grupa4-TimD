using InitialProject.Application.Commands;
using InitialProject.Application.Factories;
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
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Rezervacija uspešno kreirana.\n" +
                                "Iskoristili ste jedan bonus poen i time ostvarili popust.\n" +
                                $"Preostalo vam je {Reservation.Guest.BonusPoints} " +
                                "neiskorišćenih bonus poena.");
                else
                    MessageBox.Show("Reservation successfully created.\n" +
                                "You have used one bonus point and received a discount.\n" +
                                $"You have {Reservation.Guest.BonusPoints} " +
                                $"remaining unused bonus points.");
            }
            else
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Rezervacija uspešno kreirana.");
                else
                    MessageBox.Show("Reservation successfuly created.");
            }
            NavigateAnywhereAnytime();
        }
        private void NavigateAnywhereAnytime()
        {
            var viewModel = ViewModelFactory.Instance.CreateAnywhereAnytimeVM(_navigationStore, Guest);
            NavigationService.Instance.Navigate(viewModel);
        }
        private void NavigateImageBrowser(string imageURL)
        {
            var viewModel = ViewModelFactory.Instance.CreateImageBrowserVM
                (Reservation.Accommodation.PictureURLs, imageURL, RecreateSelf);
            NavigationService.Instance.Navigate(viewModel);
        }
        private void RecreateSelf()
        {
            var viewModel = ViewModelFactory.Instance.CreateReservationDetailsVM
                (_navigationStore, Guest, Reservation, IsReadOnlyMode);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
