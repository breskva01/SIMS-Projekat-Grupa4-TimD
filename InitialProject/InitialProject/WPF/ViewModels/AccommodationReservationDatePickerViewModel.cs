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

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationReservationDatePickerViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _service;
        private readonly NavigationStore _navigationStore;
        public AccommodationReservation SelectedReservation { get; set; }
        public List<AccommodationReservation> Reservations { get; set; }
        public int GuestNumber { get; set; }
        public ICommand ReserveCommand { get; }
        public AccommodationReservationDatePickerViewModel(NavigationStore navigationStore, List<AccommodationReservation> reservations)
        {
            _navigationStore = navigationStore;
            _service = new AccommodationReservationService();
            Reservations = reservations;
            ReserveCommand = new ExecuteMethodCommand(ReserveAccommodation);
        }

        private void ReserveAccommodation()
        {
            if (SelectedReservation == null)
                MessageBox.Show("Izaberite željeni termin.");
            else if (GuestNumber == 0)
                MessageBox.Show("Unesite broj gostiju.");
            else if (GuestNumber > SelectedReservation.Accommodation.MaximumGuests)
                MessageBox.Show("Uneti broj gostiju prelazi zadati limit.");
            else
            {
                SelectedReservation.GuestNumber = GuestNumber;
                _service.Save(SelectedReservation);
                MessageBox.Show("Rezervacija uspešno kreirana.");
                NavigateToAccommodationBrowser();
            }
        }
        private void NavigateToAccommodationBrowser()
        {
            var viewModel = new AccommodationBrowserViewModel(_navigationStore, SelectedReservation.Guest);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
    }
}
