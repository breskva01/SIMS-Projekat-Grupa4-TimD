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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class AccommodationReservationMoveRequestViewModel : ViewModelBase
    {
        private readonly AccommodationReservationRequestService _service;
        private readonly NavigationStore _navigationStore;
        public AccommodationReservation Reservation { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public ICommand SubmitRequestCommand { get; }
        public ICommand CancelCommand { get; }
        public AccommodationReservationMoveRequestViewModel(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            _navigationStore = navigationStore;
            _service = new AccommodationReservationRequestService();
            Reservation = reservation;
            SubmitRequestCommand = new ExecuteMethodCommand(SubmitRequest);
            CancelCommand = new ExecuteMethodCommand(ShowMyReservationsView);
        }
        private void SubmitRequest()
        {
            if (CheckIn != DateTime.MinValue && CheckOut != DateTime.MinValue)
            {
                DateOnly checkIn = DateOnly.FromDateTime(CheckIn);
                DateOnly checkOut = DateOnly.FromDateTime(CheckOut);
                _service.Save(Reservation, checkIn, checkOut);
                MessageBox.Show("Zahtev uspešno poslat.");
                ShowMyReservationsView();
            }
            else
                MessageBox.Show("Izaberite željene datume.");
        }
        private void ShowMyReservationsView()
        {
            var contentViewModel = new MyAccommodationReservationsViewModel(_navigationStore, Reservation.Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, Reservation.Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
    }
}
