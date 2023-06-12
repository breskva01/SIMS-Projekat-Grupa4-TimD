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
using InitialProject.Application.Factories;

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
        public ICommand NavigateReservationDetailsCommand { get; }
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
            NavigateReservationDetailsCommand = new AccommodationReservationClickCommand(NavigateReservationDetails);
        }
        private void CancelReservation(AccommodationReservation reservation)
        {
            string messageBoxText = "";
            string messageBoxCaption = "";
            if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
            {
                messageBoxText = "Da li ste sigurni da želite da otkažete rezervaciju?";
                messageBoxCaption = "Potvrda otkazivanja";
            }
            else
            {
                messageBoxText = "Are you sure you want to cancel the reservation?";
                messageBoxCaption = "Cancellation Confirmation";
            }
            MessageBoxResult result = MessageBox.Show(messageBoxText, messageBoxCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (reservation.CanBeCancelled())
                {
                    _reservationService.Cancel(reservation.Id, reservation.Accommodation.Owner.Id);
                    if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                        MessageBox.Show("Rezervacija uspešno otkazana.");
                    else
                        MessageBox.Show("Reservation successfuly canceled.");
                }
                else if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Rezervacija se ne može otkazati.");
                else
                    MessageBox.Show("This reservation can not be canceled.");
            }
        }

        private void MoveReservation(AccommodationReservation reservation)
        { 
            if (_requestService.HasPendingMoveRequest(reservation.Id))
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Već postoji zahtev za pomeranje ove rezervacije.");
                else
                    MessageBox.Show("A request to reschedule this reservation has already been submited.");
            }
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
            var viewModel = ViewModelFactory.Instance.CreateReservationMoveRequestVM(_navigationStore, reservation);
            NavigationService.Instance.Navigate(viewModel);
        }
        private void NavigateReservationDetails(AccommodationReservation reservation)
        {
            var viewModel = ViewModelFactory.Instance.CreateReservationDetailsVM(_navigationStore, _user, reservation, true);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
