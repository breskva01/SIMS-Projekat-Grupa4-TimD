using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Application.Commands;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class AccommodationReservationDatePickerViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly NavigationStore _navigationStore;
        public AccommodationReservation SelectedReservation { get; set; }
        public List<AccommodationReservation> Reservations { get; set; }
        public Accommodation Accommodation { get; set; }
        public int GuestCount { get; set; }
        public ICommand ConfirmReservationCommand { get; }
        public ICommand OpenReservationFormCommand { get; }
        public AccommodationReservationDatePickerViewModel(NavigationStore navigationStore,
            List<AccommodationReservation> reservations)
        {
            _navigationStore = navigationStore;
            _reservationService = new AccommodationReservationService();
            Reservations = reservations;
            Accommodation = reservations[0].Accommodation;
            ConfirmReservationCommand = new ExecuteMethodCommand(ReserveAccommodation);
            OpenReservationFormCommand = new ExecuteMethodCommand(NavigateReservationForm);
        }

        private void ReserveAccommodation()
        {
            if (IsValidReservation())
            {
                SelectedReservation.GuestCount = GuestCount;
                _reservationService.Save(SelectedReservation);

                if (_reservationService.IsDiscountAvailable(SelectedReservation.Guest))
                {
                    MessageBox.Show("Rezervacija uspešno kreirana.\n" +
                                    "Iskoristili ste jedan bonus poen i time ostvarili popust.\n" +
                                    $"Preostalo vam je {SelectedReservation.Guest.BonusPoints} " +
                                    "neiskorišćenih bonus poena.");
                }
                else
                {
                    MessageBox.Show("Rezervacija uspešno kreirana.");
                }

                NavigateAccommodationBrowser();
            }
        }
        private bool IsValidReservation()
        {
            if (SelectedReservation == null)
            {
                MessageBox.Show("Izaberite željeni termin.");
                return false;
            }
            if (GuestCount == 0)
            {
                MessageBox.Show("Unesite broj gostiju.");
                return false;
            }
            if (GuestCount > SelectedReservation.Accommodation.MaximumGuests)
            {
                MessageBox.Show($"Uneti broj gostiju prelazi zadati limit " +
                                $"({SelectedReservation.Accommodation.MaximumGuests})");
                return false;
            }
            return true;
        }
        private void NavigateAccommodationBrowser()
        {
            var contentViewModel = new AccommodationBrowserViewModel(_navigationStore, SelectedReservation.Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, SelectedReservation.Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
        private void NavigateReservationForm()
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, 
                Reservations[0].Guest, Reservations[0].Accommodation);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
    }
}
