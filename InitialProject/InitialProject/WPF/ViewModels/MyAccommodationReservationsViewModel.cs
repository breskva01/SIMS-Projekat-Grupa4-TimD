using InitialProject.Controller;
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

namespace InitialProject.WPF.ViewModels
{
    public class MyAccommodationReservationsViewModel : ViewModelBase, IObserver
    {
        private readonly User _loggedInUser;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        private readonly AccommodationReservationService _service;
        private readonly NavigationStore _navigationStore;
        public ICommand CancelReservationCommand { get; }
        public MyAccommodationReservationsViewModel(NavigationStore navigationStore, User loggedInUser)
        {
            _navigationStore = navigationStore;
            _loggedInUser = loggedInUser;
            _service = new AccommodationReservationService();
            Reservations = new ObservableCollection<AccommodationReservation>
                                (_service.GetConfirmed(_loggedInUser.Id));
            _service.Subscribe(this);
            CancelReservationCommand = new AccommodationReservationClickCommand(CancelReservation);
        }
        private void CancelReservation(AccommodationReservation reservation)
        {
            if (_service.Cancel(reservation.Id))
                MessageBox.Show("Rezervacija uspešno otkazana.");
            else
                MessageBox.Show("Rezervacija se ne može otkazati.");
        }
        public void Update()
        {
            var reservations = new ObservableCollection<AccommodationReservation>
                                (_service.GetConfirmed(_loggedInUser.Id));
            Reservations.Clear();
            foreach (var r in reservations)
                Reservations.Add(r);
        }
    }
}
