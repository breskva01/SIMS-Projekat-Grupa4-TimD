using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Application.Services;
using InitialProject.Application.Commands;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class MyAccommodationReservationsViewModel : ViewModelBase, IObserver
    {
        private readonly Guest1 _user;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationReservationRequestService _requestService;
        private readonly NavigationStore _navigationStore;
        public ICommand CancelReservationCommand { get; }
        public ICommand MoveReservationCommand { get; }
        public MyAccommodationReservationsViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _reservationService = new AccommodationReservationService();
            _requestService = new AccommodationReservationRequestService();
            Reservations = new ObservableCollection<AccommodationReservation>
                                (_reservationService.GetExistingGuestReservations(_user.Id));
            _reservationService.Subscribe(this);
            CancelReservationCommand = new AccommodationReservationClickCommand(CancelReservation);
            MoveReservationCommand = new AccommodationReservationClickCommand(MoveReservation);
        }
        private void CancelReservation(AccommodationReservation reservation)
        {
            MessageBoxResult result = MessageBox.Show
                ("Da li ste sigurni da želite da otkažete rezervaciju?", "Potvrda otkazivanja",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (reservation.CanBeCancelled())
                {
                    _reservationService.Cancel(reservation.Id, reservation.Accommodation.Owner.Id);
                    MessageBox.Show("Rezervacija uspešno otkazana.");
                } 
                else
                    MessageBox.Show("Rezervacija se ne može otkazati.");
            }
        }

        private void MoveReservation(AccommodationReservation reservation)
        { 
            if (_requestService.HasPendingMoveRequest(reservation.Id))
                MessageBox.Show("Već postoji zahtev za pomeranje ove rezervacije.");
            else
                ShowMoveRequestPrompt(reservation);
        }
        public void Update()
        {
            var reservations = new ObservableCollection<AccommodationReservation>
                                (_reservationService.GetExistingGuestReservations(_user.Id));
            Reservations.Clear();
            foreach (var r in reservations)
                Reservations.Add(r);
        }
        private void ShowMoveRequestPrompt(AccommodationReservation reservation)
        {
            var viewModel = new AccommodationReservationMoveRequestViewModel(_navigationStore, reservation);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
