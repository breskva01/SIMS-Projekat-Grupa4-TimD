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

namespace InitialProject.WPF.ViewModels.Guest1
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
            OpenReservationFormCommand = new ExecuteMethodCommand(ShowReservationForm);
        }

        private void ReserveAccommodation()
        {
            if (SelectedReservation == null)
                MessageBox.Show("Izaberite željeni termin.");
            else if (GuestCount == 0)
                MessageBox.Show("Unesite broj gostiju.");
            else if (GuestCount > SelectedReservation.Accommodation.MaximumGuests)
                MessageBox.Show($"Uneti broj gostiju prelazi zadati limit " +
                    $"({SelectedReservation.Accommodation.MaximumGuests})");
            else
            {
                SelectedReservation.GuestCount = GuestCount;
                _reservationService.Save(SelectedReservation);
                MessageBox.Show("Rezervacija uspešno kreirana.");
                NavigateAccommodationBrowser();
            }
        }
        private void NavigateAccommodationBrowser()
        {
            var contentViewModel = new AccommodationBrowserViewModel(_navigationStore, SelectedReservation.Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, SelectedReservation.Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
        private void ShowReservationForm()
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, 
                Reservations[0].Guest, Reservations[0].Accommodation);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
    }
}
